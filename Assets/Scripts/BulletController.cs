using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float deltaStartingRotation;

    private Rigidbody2D myRigidbody2D;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();

        LaunchBullet();
    }
    private void LaunchBullet()
    {
        myRigidbody2D.velocity = TranslateRotationIntoVector2(transform.rotation.eulerAngles.z) * bulletSpeed;
    }

    private Vector2 TranslateRotationIntoVector2(float rotationToTranslate)
    {
        float xLength = -Mathf.Sin((rotationToTranslate + deltaStartingRotation) * Mathf.Deg2Rad);
        float yLength = Mathf.Cos((rotationToTranslate + deltaStartingRotation) * Mathf.Deg2Rad);


        Vector2 returnVector = new Vector3(xLength, yLength);
        return returnVector;
    }
}
