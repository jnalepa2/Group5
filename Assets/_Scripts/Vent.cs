using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject thePlayer;

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggerGO = other.gameObject;
        if (triggerGO.tag == "Player") {
            thePlayer.transform.position = teleportTarget.transform.position;
        }
    }

}
