using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FreeGridSpaceOnDestroyed : MonoBehaviour
{
    Vector2 currentPositionOnGrid;
    ObstacleSpawner obstacleSpawner;
    private void OnDestroy()
    {
        //Count current position
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        currentPositionOnGrid = new Vector2(transform.position.x, transform.position.y);
        //Modify grid position lists
        obstacleSpawner.RemoveObstaclePositionFromList(currentPositionOnGrid);
        obstacleSpawner.AddEmptyPositionToList(currentPositionOnGrid);
    }
}
