using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RespawnOnDeath : MonoBehaviour
{
    SpawnPlayers spawnPlayers;
    private void Start()
    {
        spawnPlayers = FindObjectOfType<SpawnPlayers>();
    }
    public void ActWhenDestroyed()
    {
        StartCoroutine(spawnPlayers.SpawnPlayer());
    }
}
