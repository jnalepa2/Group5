using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Wall" || otherGO.tag == "Door" || otherGO.tag == "LockedDoor") {
            Destroy(gameObject);
        }
        else if (otherGO.tag == "Enemy") {
            Destroy(gameObject);
            //call enemy script's function to decrement their health
			
			otherGO.GetComponent<Enemy>().takeDamage();
        }
    }
}
