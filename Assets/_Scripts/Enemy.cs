using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Set in Inspector")]
    public float speed = 30;
	public bool canMove = true;
	public bool canShoot = false;
	public GameObject visionZone;
	
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
        if(alive)
		{
				detectPlayer();
			
				if(canMove){
					if(playerSight == true){
						moveToPlayer();
					}
					else{
						returnToStart();
					}
				}
		}
    }
	
	void FixedUpdate()
	{
		
	}
	
	IEnumerator OnCollisionStay( Collision coll ) {
		GameObject collidedWith = coll.gameObject;
		if ( collidedWith.tag != "Ground" )
		{
			alive = false;
			if ( collidedWith.tag == "Wall" )
			{
				returnToStart();
				yield return new WaitForSeconds (1);
			}
			else if ( collidedWith.tag == "Player" )
			{
				returnToStart(1);
				yield return new WaitForSeconds (2);
			}
			alive = true;
		}
	}
	
	IEnumerator OnCollisionEnter( Collision coll )
	{
		GameObject collidedWith = coll.gameObject;
		if ( collidedWith.tag != "Ground" )
		{
			alive = false;
			if ( (collidedWith.tag == "Wall" ) || (collidedWith.tag == "Door" ) || (collidedWith.tag == "LockedDoor" ) ) {
				returnToStart();
				yield return new WaitForSeconds (1);
			}
			else if ( collidedWith.tag == "Player" ) {
				returnToStart(1);
				yield return new WaitForSeconds (2);
			}
			alive = true;
		}
	}
	
	void detectPlayer(){
			if (thePlayer == null)
			{
				playerSight = false;
				return;
			}
			else if(visionZone.GetComponent<EnemySight>().IsIn())
				playerSight = true;
			else
				playerSight = false;
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
	
	void returnToStart(float forcedSpeed){
		Vector3 current = transform.position;
		
		current.x += (forcedSpeed/8)  * Time.deltaTime * (enemyStartPos.x - current.x);
        current.z += (forcedSpeed/8) * Time.deltaTime * (enemyStartPos.z - current.z);
		
        transform.position = current;
	}
	
	public void takeDamage()
	{
		this.health -= 8;
		if(health <= 0)		alive = false;
		if(!alive)			Destroy(gameObject);
	}
}
