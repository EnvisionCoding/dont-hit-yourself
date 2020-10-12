using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance_BlackHole : MonoBehaviour
{
    public Transform[] exits;
    public Transform testExit;

    [Header("Projectile Editing")]
    public float homingSpeed = 8;
    public float rotationSpeed = 350;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileHoming" ||
            collision.gameObject.tag == "Projectile" ||
            collision.gameObject.tag == "ProjectileElectric")
        {
            if (collision.gameObject.GetComponent<Projectiles>().isFromBlackhole == false)
            {
                collision.gameObject.GetComponent<Projectiles>().UpdateTarget(gameObject.transform);
                collision.gameObject.GetComponent<Projectiles>().SetBlackHoleSpeed(8, 350);
            }


        }
    }
}
