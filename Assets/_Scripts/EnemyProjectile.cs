using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    void OnTriggerEnter(Collider coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Wall" || otherGO.tag == "Door" || otherGO.tag == "LockedDoor") {
            Destroy(gameObject);
        }
        else if (otherGO.tag == "Player") {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
