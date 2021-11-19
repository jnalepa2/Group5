﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from Hero.cs from Chapter 30, Space SHMUP, Introduction to Game Design, Prototyping, and Development by Jeremy Gibson Bond
public class Player : MonoBehaviour {
    static public Player S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float force = 500;
    public float enemyProjectileDamage = 5;
    public GameObject projectilePrefab;
    public GameObject gunEnd;

    [Header("Set Dynamically")]
    public float health = 20;

    void Awake() {
        if (S == null) {
            S = this;
        }
        else {
            Debug.LogError("Player.Awake() Attempted to assign a second Player.S!");
        }
    }

    void Update() {
        //Move player based on keyboard input  
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.z += zAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Rotate player so gun points toward the mouse position 
        //Adapted from "Bogaland" at https://forum.unity.com/threads/2d-character-rotation-towards-mouse.457126/
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg * -1 + 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

        //Player can shoot with mouse button
        if (Input.GetMouseButtonDown(0)) {
            FireGun(angle);
        }
    }

    void FireGun(float angle) {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = gunEnd.transform.position;
        //projGO.transform.Translate(0, 0, 1);
        projGO.transform.Rotate(0, angle, 0);
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.AddRelativeForce(projGO.transform.forward * force);

    }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "ProjectileEnemy") {
            Destroy(otherGO);
            health -= enemyProjectileDamage;

            if (health <= 0) {
                Destroy(gameObject);
            }
        }

    }
}
