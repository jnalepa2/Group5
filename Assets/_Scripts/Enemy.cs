﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Set in Inspector")]
    public float speed = 30;
	public float sightRange = 20;
	
	[Header("Set Dynamically")]
	public float health = 3;
	public bool alive = true;				//Used to set the enemy to active or inactive so it behaves better
	public bool playerSight = false;		//If the player is in range of vision
	
	private GameObject thePlayer;
	private Vector3 enemyStartPos;
	
	
    void Start()
    {
		thePlayer = GameObject.FindGameObjectsWithTag("Player")[0];
		enemyStartPos  = transform.position;
    }

    void Update()
    {
        if(alive){
			if(thePlayer != null)		detectPlayer();
			
				if(playerSight == true){
					moveToPlayer();
				}
				else{
					returnToStart();
				}
		}
    }
	
	void FixedUpdate()
	{
		GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
		if(Players == null)			thePlayer = null;
	}
	
	IEnumerator OnCollisionStay( Collision coll ) {
		
		GameObject collidedWith = coll.gameObject;
        if ( collidedWith.tag == "Wall" ) {
			alive = false;
			yield return new WaitForSeconds (1);
			alive = true;
		}
		else if ( collidedWith.tag == "Player" ) {
			alive = false;
			yield return new WaitForSeconds (2);
			alive = true;
		}
	}
	
	void detectPlayer(){
			float distance = Vector3.Distance(thePlayer.transform.position, transform.position);
			
			if( distance < sightRange )
			{ playerSight = true; }
			else 
			{ playerSight = false; }
	}
	
	void moveToPlayer(){
		Vector3 current = transform.position;
		Vector3 destination = thePlayer.transform.position;
		
		current.x += speed * Time.deltaTime * (destination.x - current.x);
        current.z += speed * Time.deltaTime * (destination.z - current.z);
		
        transform.position = current;
	}
	
	void returnToStart(){
		Vector3 current = transform.position;
		
		current.x += (speed/4)  * Time.deltaTime * (enemyStartPos.x - current.x);
        current.z += (speed/4) * Time.deltaTime * (enemyStartPos.z - current.z);
		
        transform.position = current;
	}
	
	public void takeDamage()
	{
		this.health -= 8;
		if(health <= 0)		alive = false;
		if(!alive)			Destroy(gameObject);
	}
}
