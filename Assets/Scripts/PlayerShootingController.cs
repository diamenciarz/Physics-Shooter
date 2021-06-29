using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    [Header("Shooting Stats")]
    [SerializeField] Transform shootingPoint;
    [SerializeField] float deltaRotation;
    [Header("Bullet Stats")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootingCooldown;

    private float lastShotTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForShooting();
    }
    private void CheckForShooting()
    {
        if ((Time.time - lastShotTime) > shootingCooldown)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        lastShotTime = Time.time;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + deltaRotation);
        Instantiate(bulletPrefab, shootingPoint.transform.position, spawnRotation);
    }
}
