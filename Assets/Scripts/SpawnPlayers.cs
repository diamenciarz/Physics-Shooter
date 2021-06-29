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

    public void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerToSpawnPrefab.name, randomPosition, Quaternion.identity);
        
    }
}
