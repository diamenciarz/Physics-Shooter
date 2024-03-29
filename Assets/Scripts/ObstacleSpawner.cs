using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObstacleSpawner : MonoBehaviour 
{
    [Header("Spawn Settings")]
    [SerializeField][Range(0,100)] int spawnPercentageChance;
    [SerializeField] List<GameObject> objectsToSpawnList;
    [SerializeField][Min(0)] int spawnNewObstaclesIfBelow;
    [SerializeField] float spawnCooldown;
    [SerializeField] GameObject indestructibleWallPrefab;
    [Header("Grid Settings")]
    [SerializeField] int xCount = 10;
    [SerializeField] int yCount = 10;
    [SerializeField] float gridX = 1.68f;
    [SerializeField] float gridY= 1.68f;


    public List<GameObject> obstacleList;
    public List<Vector2> obstaclePositionList;
    public List<Vector2> emptyPositionList;
    Coroutine spawnNewObstaclesCoroutine;
    //Privat variab
    private PhotonView photonView;
    private SpawnPlayers spawnPlayers;
    [HideInInspector]
    public bool isMapGenerated;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        spawnPlayers = FindObjectOfType<SpawnPlayers>();

        if (PhotonNetwork.IsMasterClient)
        {
            FillEmptyPositionList();
            SpawnObstaclesInASquare();
            SorroundMapWithTheIndestructibleWall();

            spawnPlayers.GetComponent<PhotonView>().RPC("MapHasBeenGenerated", RpcTarget.AllBuffered);

            spawnNewObstaclesCoroutine = StartCoroutine(SpawnNewObstaclesOnGrid());
        }
    }
    private void FillEmptyPositionList()
    {
        for (int i = 0; i < xCount; i++)
        {
            for (int k = 0; k < yCount; k++)
            {
                Vector3 emptyPosition = new Vector3(gridX * i, gridY * k, 0);
                AddEmptyPositionToList(emptyPosition);
            }
        }
    }
    private void SpawnObstaclesInASquare()
    {
        for (int i = 0; i < xCount; i++)
        {
            for (int k = 0; k < yCount; k++)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < spawnPercentageChance)
                {
                    Vector3 spawnPosition = new Vector3(gridX * i, gridY * k, 0);
                    GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];

                    //Spawn obstacle
                    SpawnObstacleOnLocation(spawnPosition, objectToSpawn);
                }
            }
        }
    }
    private void SpawnObstacleOnLocation(Vector3 spawnPosition, GameObject objectToSpawn)
    {
        //Spawn the random object at this position
        GameObject spawnedObject = PhotonNetwork.Instantiate(objectToSpawn.name, spawnPosition, Quaternion.identity);

        //Add stats of the spawned object to lists
        AddObstacleGameObjectToList(spawnedObject);
        AddObstaclePositionToList(spawnPosition);
        RemoveEmptyPositionFromList(spawnPosition);
    }
    private void SorroundMapWithTheIndestructibleWall()
    {
        for (int i = -1; i < xCount + 1; i++)
        {
            Vector3 spawnPosition = new Vector3(gridX * i, gridY * (yCount + 1), 0);
            GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];
            //Spawn the random object at this position
            GameObject spawnedObject = PhotonNetwork.Instantiate(indestructibleWallPrefab.name, spawnPosition, Quaternion.identity);
        }
        for (int i = -1; i < xCount + 1; i++)
        {
            Vector3 spawnPosition = new Vector3(gridX * i, gridY * (-1), 0);
            GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];
            //Spawn the random object at this position
            GameObject spawnedObject = PhotonNetwork.Instantiate(indestructibleWallPrefab.name, spawnPosition, Quaternion.identity);
        }

        for (int i = 0; i < yCount + 1; i++)
        {
            Vector3 spawnPosition = new Vector3(gridX * xCount, gridY * i, 0);
            GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];
            //Spawn the random object at this position
            GameObject spawnedObject = PhotonNetwork.Instantiate(indestructibleWallPrefab.name, spawnPosition, Quaternion.identity);
        }
        for (int i = 0; i < yCount + 1; i++)
        {
            Vector3 spawnPosition = new Vector3(gridX  * (-1), gridY * i, 0);
            GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];
            //Spawn the random object at this position
            GameObject spawnedObject = PhotonNetwork.Instantiate(indestructibleWallPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
    IEnumerator SpawnNewObstaclesOnGrid()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            if (obstacleList.Count < spawnNewObstaclesIfBelow)
            {
                Vector3 spawnPosition = ReturnRandomSpawnBlockPositionOnTheGrid();
                GameObject objectToSpawn = objectsToSpawnList[Random.Range(0, objectsToSpawnList.Count)];

                //Spawn obstacle
                SpawnObstacleOnLocation(spawnPosition, objectToSpawn);
            }
        }
    }

    public void AddObstaclePositionToList(Vector2 positionToAdd)
    {
        obstaclePositionList.Add(positionToAdd);
    }
    public void RemoveObstaclePositionFromList(Vector2 positionToRemove)
    {
        if (obstaclePositionList.Contains(positionToRemove))
        {
            obstaclePositionList.Remove(positionToRemove);
        }
    }
    public bool CheckIfObstaclePositionListContains(Vector2 positionToCheck)
    {
        if (obstaclePositionList.Contains(positionToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddEmptyPositionToList(Vector2 positionToAdd)
    {
        emptyPositionList.Add(positionToAdd);
    }
    public void RemoveEmptyPositionFromList(Vector2 positionToRemove)
    {
        if (emptyPositionList.Contains(positionToRemove))
        {
            emptyPositionList.Remove(positionToRemove);
        }
    }
    public bool CheckIfEmptyPositionListContains(Vector2 positionToCheck)
    {
        if (emptyPositionList.Contains(positionToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddObstacleGameObjectToList(GameObject objectToAdd)
    {
        obstacleList.Add(objectToAdd);
    }
    public void RemoveObstacleFromList(GameObject obstacleToRemove)
    {
        if (obstacleList.Contains(obstacleToRemove))
        {
            obstacleList.Remove(obstacleToRemove);
        }
    }
    public bool CheckIfObstacleListContains(GameObject obstacleToCheck)
    {
        if (obstacleList.Contains(obstacleToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2 ReturnRandomCenterPositionOnTheGrid()
    {
        Vector2 vectorToReturn = emptyPositionList[Random.Range(0, emptyPositionList.Count)];
        vectorToReturn += new Vector2(gridX, gridY);
        
        return vectorToReturn;
    }
    public Vector2 ReturnRandomSpawnBlockPositionOnTheGrid()
    {
        Vector2 vectorToReturn = emptyPositionList[Random.Range(0, emptyPositionList.Count)];

        return vectorToReturn;
    }

    public float GetGridX()
    {
        return gridX;
    }
    public float GetGridY()
    {
        return gridY;
    }
    public float GetXCount()
    {
        return xCount;
    }
    public float GetYCount()
    {
        return yCount;
    }
}
