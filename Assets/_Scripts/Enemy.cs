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
	public GameObject additionalVision;
	public float fireRate = 2;
	public float projectileForce = 500;
	public GameObject projectilePrefab;
	public AudioSource firing;
	public AudioSource death;
	
	[Header("Set Dynamically")]
	public float health = 3;
	public bool alive = true;				//Used to set the enemy to active or inactive so it behaves better
	public bool playerSight = false;		//If the player is in range of vision
	public float nextFire = 0;
	
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
					if(playerSight){
						moveToPlayer();
					}
					else{
						returnToStart();
					}
				}
				
				if(canShoot)
				{
					if(playerSight)
					{
						if (Time.time > nextFire)
						{
							//update time
							nextFire = Time.time + fireRate;
							
							Vector3 pos = transform.position;

							GameObject projGO = Instantiate<GameObject>(projectilePrefab);
							projGO.GetComponent<EnemyProjectile>().firingTarget = gameObject;
							
							projGO.transform.position = transform.position;
							
							Vector3 direction = -( pos - thePlayer.transform.position );
							//place the newly created projectile at the end of the player's gun

							//rotate the projectile to point toward direction of aiming
							//projGO.transform.Rotate(0, angle, 0);

							//apply force to projectile to fire it
							Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
							rigidB.AddRelativeForce(direction * projectileForce);
							firing.Play();
						}
					}
				}
		}
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
	
	IEnumerator OnCollisionEnter( Collision coll ){
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
			else if( (visionZone.GetComponent<EnemySight>().IsIn()) || ( (additionalVision != null) && additionalVision.GetComponent<EnemySight>().IsIn()) )
			{
				playerSight = true;
			}
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
	
	public void takeDamage(){
		this.health -= 8;
		if(health <= 0)		alive = false;
		if(!alive){
			AudioSource deathSound = Instantiate<AudioSource>(death);
			deathSound.transform.position = transform.position;
			deathSound.Play();
			Destroy(gameObject);
		}
	}
	
	public IEnumerator wait(float sec)
	{
		yield return new WaitForSeconds(sec);
	}
}
