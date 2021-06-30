using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player following stats")]
    public float turnMarginDampener;
    [SerializeField] float screenEdgeOffset = 0.5f;
    public GameObject followGameObject;
    [Header("Stats useful for others")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float xScreenLength;
    public float yScreenLength;

    //Private variables
    private Vector3 edgeOfMapVector;
    private Vector3 cameraVelocity;
    private Vector3 lastPositionWhenObjectWasAlive;
    private float lastTimeWhenObjectWasAlive;
    ObstacleSpawner obstacleSpawner;
    Camera mainCamera;
    void Start()
    {
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

        mainCamera = Camera.main;
        xScreenLength = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0, 0)).x;
        yScreenLength = mainCamera.ViewportToWorldPoint(new Vector3(0, 1f, 0)).y;

        SetUpScreenBorders();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEdgeOfMap();
        CountAndSetNewCameraPosition();
    }
    private void SetUpScreenBorders()
    {
        Camera mainCamera = Camera.main;
        xMin = -screenEdgeOffset;
        xMax = (obstacleSpawner.GetGridX() * obstacleSpawner.GetXCount()) + screenEdgeOffset;

        yMin = -screenEdgeOffset;
        yMax = (obstacleSpawner.GetGridY() * obstacleSpawner.GetYCount()) + screenEdgeOffset;
    }
    private void CheckForEdgeOfMap()
    {
        if (followGameObject != null)
        {
            edgeOfMapVector = CountEdgeVectorForMovement();
        }
    }
    private Vector3 CountEdgeVectorForMovement()
    {
        if (followGameObject != null)
        {
            Vector3 edgeVector = new Vector3(0, 0, 0);
            //Policz wspó³rzêdne prostych, za którymi nale¿y zacz¹æ lekko zwalniaæ
            float yEdgeMargin = yMax - (yScreenLength * turnMarginDampener);
            float xEdgeMargin = xMax - (xScreenLength * turnMarginDampener);
            //Debug.Log("Edge Margin Y: " + yEdgeMargin);
            if (Mathf.Abs(followGameObject.transform.position.y) > yEdgeMargin)
            {
                //
                edgeVector.y = (Mathf.Abs(followGameObject.transform.position.y) - yEdgeMargin) / (yScreenLength * turnMarginDampener);
                if (followGameObject.transform.position.y > 0)
                {
                    edgeVector.y = -edgeVector.y;
                }
            }
            if (Mathf.Abs(followGameObject.transform.position.x) > xEdgeMargin)
            {
                edgeVector.x = (Mathf.Abs(followGameObject.transform.position.x) - xEdgeMargin) / (xScreenLength * turnMarginDampener);
                if (followGameObject.transform.position.x > 0)
                {
                    edgeVector.x = -edgeVector.x;
                }
            }
            return edgeVector;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }
    private void CountAndSetNewCameraPosition()
    {
        if (followGameObject != null)
        {
            cameraVelocity = CountCameraVelocity();
            Vector3 moveTowardsResultVector = new Vector3(0, 0, 0);
            moveTowardsResultVector = edgeOfMapVector;

            transform.position = followGameObject.transform.position + new Vector3(moveTowardsResultVector.x * xScreenLength, moveTowardsResultVector.y * yScreenLength, -10);
            lastPositionWhenObjectWasAlive = transform.position;
            lastTimeWhenObjectWasAlive = Time.time;
        }
        else
        {
            float timeSinceObjectWasDestroyed = Time.time - lastTimeWhenObjectWasAlive;
            transform.position = lastPositionWhenObjectWasAlive + cameraVelocity * Mathf.Sqrt(Time.time - timeSinceObjectWasDestroyed);
        }
    }
    private Vector3 CountCameraVelocity()
    {
        Vector3 returnVector;

        returnVector = (lastPositionWhenObjectWasAlive - transform.position) / Time.deltaTime;

        return returnVector;
    }
}
