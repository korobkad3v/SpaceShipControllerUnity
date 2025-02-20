using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;

    private void Awake()
    {
        fractured.transform.localScale = gameObject.transform.localScale;

        foreach (Rigidbody RBs in fractured.GetComponentsInChildren<Rigidbody>())
        {
            
            RBs.useGravity = false;
            RBs.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    public void FractureObject()
    {
        Instantiate(fractured, transform.position, transform.rotation); 
        
        Destroy(gameObject); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        FractureObject();
    }
}
