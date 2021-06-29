using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed;

    private Rigidbody2D myRigidbody2D;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();

        LaunchBullet();
    }
    private void LaunchBullet()
    {
        myRigidbody2D.velocity = TranslateRotationIntoVector3(transform.rotation.eulerAngles.z) * bulletSpeed;
    }

    private Vector3 TranslateRotationIntoVector3(float rotationToTranslate)
    {
        float xLength = -Mathf.Sin(rotationToTranslate);
        float yLength = Mathf.Cos(rotationToTranslate);

        Vector3 returnVector = new Vector3(xLength, yLength, 0);
        return returnVector;
    }
}
