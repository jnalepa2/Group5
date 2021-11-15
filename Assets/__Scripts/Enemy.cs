using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Set in Inspector")]
    public float speed = 30;
	public float sightRange = 20;
	
	[Header("Set Dynamically")]
	public float health = 3;
	
	private bool playerSight = false;
	private GameObject thePlayer;
	
	
    // Start is called before the first frame update
    void Start()
    {
		thePlayer = GameObject.FindGameObjectsWithTag("Player")[0];
		detectPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();
		if(playerSight == true)
			moveToPlayer();
    }
	
	/*void OnCollisionEnter( Collision coll ) {
		
		GameObject collidedWith = coll.gameObject;
        if ( collidedWith.tag == "Player" ) {
			Player.health -= 1;
		}
	} */
	
	void detectPlayer(){
		//Vector3 position = transform.position;
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
}
