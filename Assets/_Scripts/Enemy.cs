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
	private Vector3 enemyStartPos;
	
	
    // Start is called before the first frame update
    void Start()
    {
		thePlayer = GameObject.FindGameObjectsWithTag("Player")[0];
		enemyStartPos  = transform.position;
		detectPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();
		if(playerSight == true){
			moveToPlayer();
		}
		else{
			returnToStart();
		}
    }
	
	void OnCollisionEnter( Collision coll ) {
		
		GameObject collidedWith = coll.gameObject;
        if ( collidedWith.tag == "Wall" ) {
			StopCoroutine(blind());
			StartCoroutine(blind());
		}
		else if ( collidedWith.tag == "Player" ) {
			//thePlayer.health -= 1;
		} 
	}
	
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
	
	void returnToStart(){
		Vector3 current = transform.position;
		
		current.x += (speed/4)  * Time.deltaTime * (enemyStartPos.x - current.x);
        current.z += (speed/4) * Time.deltaTime * (enemyStartPos.z - current.z);
		
        transform.position = current;
	}
	
	IEnumerator blind(){ //temporarily reduces the enemy sight range, to get it to stop chasing
		float oldSight = sightRange;
		sightRange = 0;
		yield return new WaitForSeconds (3);
		sightRange = oldSight;
		/*if(playerSight == false){
			StopCoroutine(returnToStart());
			StartCoroutine(returnToStart());
		}*/
	}
}
