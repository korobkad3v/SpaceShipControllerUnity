using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Asteroid")
        {
            var fracture = collision.gameObject.GetComponent<Fracture>();
            if (fracture != null)
            {
                fracture.FractureObject();
            }
        }

        if (collision.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
        
    }
}
