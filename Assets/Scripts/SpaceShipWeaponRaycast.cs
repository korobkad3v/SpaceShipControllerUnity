using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipWeaponRaycast : MonoBehaviour
{
    public float damage = 25f;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;

    public Transform gunEnd;

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private float nextFire;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void Fire()
    {
        StartCoroutine(ShotEffect());
    }

    private IEnumerator ShotEffect()

    {
        laserLine.enabled = true;
        yield return shotDuration;


        laserLine.enabled = false;


    }
}
