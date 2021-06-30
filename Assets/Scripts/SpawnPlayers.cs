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
    public void Start()
    {
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        //Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        Vector2 randomPosition = obstacleSpawner.ReturnRandomCenterPositionOnTheGrid();
        Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0);
        PhotonNetwork.Instantiate(playerToSpawnPrefab.name, spawnPosition, Quaternion.identity);
    }
}
