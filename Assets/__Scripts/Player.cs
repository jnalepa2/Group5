﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from Hero.cs from Chapter 30, Space SHMUP, Introduction to Game Design, Prototyping, and Development by Jeremy Gibson Bond
public class Player : MonoBehaviour {
    static public Player S;
    private Rigidbody playerRigid;

    [Header("Set in Inspector")]
    public float moveSpeed = 30;
    public float projectileForce = 500;
    public float enemyProjectileDamage = 5;
    public float fireRate = 2;
    public float ammoPack = 10;
    public GameObject projectilePrefab;
    public GameObject gunEnd;

    [Header("Set Dynamically")]
    public float health = 20;
    public float ammo = 20;
    public float nextFire = 0;

    void Awake() {
        if (S == null) {
            S = this;
            playerRigid = gameObject.GetComponent<Rigidbody>();
        }
        else {
            Debug.LogError("Player.Awake() Attempted to assign a second Player.S!");
        }
    }

    void Update() {
        //Move player based on keyboard input  
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        //use rigid body forces to move player to prevent player from moving through walls and other objects.
        Vector3 moveInput = new Vector3(xAxis, 0, zAxis) * moveSpeed;
        playerRigid.AddForce(moveInput);

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
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire) {
            FireGun(angle);
        }
    }

    void FireGun(float angle) {
        if (ammo > 0) {
            //update time
            nextFire = Time.time + fireRate;

            GameObject projGO = Instantiate<GameObject>(projectilePrefab);

            //place the newly created projectile at the end of the player's gun
            projGO.transform.position = gunEnd.transform.position;

            //rotate the projectile to point toward direction of aiming
            projGO.transform.Rotate(0, angle, 0);

            //apply force to projectile to fire it
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
            rigidB.AddRelativeForce(projGO.transform.forward * projectileForce);

            //use ammo
            ammo -= 1;
        }

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

    //Update this method to add player interactions with items
    void OnCollisionStay(Collision coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Ammo" && Input.GetKeyDown("space")) {
            Destroy(otherGO);
            ammo += ammoPack;
        }
    }
}
