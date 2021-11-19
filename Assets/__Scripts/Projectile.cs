using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    void OnCollisionEnter( Collision coll ) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Wall") {
            Destroy(gameObject);
        }
    }
}
