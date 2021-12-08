using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag != "Ground" && otherGO.tag != "Player") {
            Destroy(gameObject);
        }

        if (otherGO.tag == "Enemy") {
            //call enemy script's function to decrement their health
			
			otherGO.GetComponent<Enemy>().takeDamage();
        }
        else if (otherGO.tag == "Box")
        {
            Destroy(otherGO);
        }
    }
}
