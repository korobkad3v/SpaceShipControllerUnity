using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipWeapon : MonoBehaviour
{
    [Header("Weapon Prop")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform projectilSpawnPos;
    [SerializeField] public float projectileSpeed = 10f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] public float nextFireTime = 0f;
    [SerializeField] public float projectileLifetime = 5f;

    [SerializeField] public float projectileBaseSpeed = 10f;

    public void Fire()
    {
        if (Time.time >= nextFireTime) 
        {
            nextFireTime = Time.time + fireRate;
            GameObject projectile = Instantiate(projectilePrefab, projectilSpawnPos.position, projectilSpawnPos.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = projectilSpawnPos.forward * projectileSpeed;

            Destroy(projectile, projectileLifetime);
        }

        
    }
}
