using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerToSpawnPrefab;
    
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    private ObstacleSpawner obstacleSpawner;
    private CameraController cameraController;
    private PhotonView photonView;
    DataCollector dataCollector;
    private bool isMapGenerated;
    public void Start()
    {
        dataCollector = FindObjectOfType<DataCollector>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        cameraController = FindObjectOfType<CameraController>();
        photonView = GetComponent<PhotonView>();

        StartCoroutine(SpawnPlayer());
    }
    public IEnumerator SpawnPlayer()
    {
        Debug.Log("Map: " + isMapGenerated);
        yield return new WaitUntil(() => isMapGenerated);

        Vector2 randomPosition = obstacleSpawner.ReturnRandomCenterPositionOnTheGrid();
        Debug.Log("Random position: " + randomPosition);
        Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0);
        GameObject createdPlayerGO = PhotonNetwork.Instantiate(playerToSpawnPrefab.name, spawnPosition, Quaternion.identity);
        Debug.Log("Created GO: " + createdPlayerGO);
        //Make this camera follow this player
        cameraController.followGameObject = createdPlayerGO;
        //Add player to list
        dataCollector.AddPlayerToList(createdPlayerGO);
    }
    [PunRPC]
    public void MapHasBeenGenerated()
    {
        isMapGenerated = true;
    }
}
