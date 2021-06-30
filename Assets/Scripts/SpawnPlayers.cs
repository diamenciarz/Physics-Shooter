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
    public void Start()
    {
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        cameraController = FindObjectOfType<CameraController>();
        photonView = GetComponent<PhotonView>();

        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        //Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        Vector2 randomPosition = obstacleSpawner.ReturnRandomCenterPositionOnTheGrid();
        Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0);
        GameObject createdPlayerGO = PhotonNetwork.Instantiate(playerToSpawnPrefab.name, spawnPosition, Quaternion.identity);
        //Make this camera follow this player
        cameraController.followGameObject = createdPlayerGO;
    }
}
