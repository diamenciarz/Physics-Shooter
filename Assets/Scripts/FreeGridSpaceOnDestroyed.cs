using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FreeGridSpaceOnDestroyed : MonoBehaviour
{
    Vector2 currentPositionOnGrid;
    ObstacleSpawner obstacleSpawner;
    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

    }
    private void OnDestroy()
    {
        //photonView.RPC("RemoveMyPositionFromList", RpcTarget.AllBuffered);
        Debug.Log("Destroyed obstacle");
        RemoveMyPositionFromList();
        
    }
    
    private void RemoveMyPositionFromList()
    {
        //Count current position
        currentPositionOnGrid = new Vector2(transform.position.x, transform.position.y);
        //Modify lists
        obstacleSpawner.RemoveObstaclePositionFromList(currentPositionOnGrid);
        obstacleSpawner.RemoveObstacleFromList(gameObject);
        obstacleSpawner.AddEmptyPositionToList(currentPositionOnGrid);
    }
}
