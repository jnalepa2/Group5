using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from FollowCam.cs from Chapter 29, Mission Demolition, Introduction to Game Design, Prototyping, and Development by Jeremy Gibson Bond

public class FollowCam : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject player;

    [Header("Set Dynamically")]
    public float camY;
    
    void Awake() {
        camY = this.transform.position.y;
    }

    void FixedUpdate() {
        if (player == null)
            return;

        Vector3 destination = player.transform.position;
        destination.y = camY;
        transform.position = destination;
    }
}
