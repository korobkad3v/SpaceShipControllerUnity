using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipWeaponManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpaceShipMovement spaceShipMovement = null;

    [Header("SpaceShip Weapons")]
    [SerializeField]
    [Tooltip("Weapons of the spaceship")]
    private SpaceShipWeapon[] spaceshipWeapons = null;

    [Header("Input system")]
    [SerializeField]
    [Tooltip("Key to shoot")]
    private KeyCode keyToShoot = KeyCode.Mouse0;

    void Awake()
    {
        if (spaceshipWeapons == null || spaceshipWeapons.Length == 0)
            Debug.Log(name + "No spaceship weapon assigned!");
    }

    void FixedUpdate()
    {
        if (Input.GetKey(keyToShoot))
        {
            foreach (var weapon in spaceshipWeapons)
            {
                weapon.projectileSpeed = weapon.projectileBaseSpeed + (spaceShipMovement.thrust * spaceShipMovement.maxSpeed * spaceShipMovement.forceMult) ;
                weapon.Fire();
            }
                
        }
    }
}
