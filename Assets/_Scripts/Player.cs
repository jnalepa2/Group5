using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Adapted from Hero.cs from Chapter 30, Space SHMUP, Introduction to Game Design, Prototyping, and Development by Jeremy Gibson Bond
public class Player : MonoBehaviour
{
    static public Player S;
    private Rigidbody playerRigid;

    [Header("Set in Inspector")]
    public float moveSpeed = 30;
    public float projectileForce = 500;
    public float enemyProjectileDamage = 5;
    public float fireRate = 2;
    public float ammoPack = 10;
    public int livesRemaining = 2;
    public bool hasKey1 = false;
    public bool hasKey2 = false;
    public GameObject projectilePrefab;
    public GameObject gunEnd;
	
	public GameObject damage;
    public GameObject key1;
    public GameObject key2;
    public GameObject scrap;
    public GameObject startScreen;
    public HandleDeath handleDeath;

    //health, ammo and money display
    public Text healthText;
    public Text ammoText;
    public Text moneyText;
    public Text controlPopupText;

    [Header("Set Dynamically")]
    public float health = 20;
    public float ammo = 20;
    public float money = 0;
    public float nextFire = 0;
    public string controlPopupMessage = "";
  
	void Start()
    {
        if (livesRemaining == 2) {
            Time.timeScale = 0;
        }
    }

    void Awake()
    {
        if (S == null)
        {
            S = this;
            playerRigid = gameObject.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Player.Awake() Attempted to assign a second Player.S!");
        }
    }

    void Update()
    {
		if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
        {
            Time.timeScale = 1;
            Destroy(startScreen);
        }
		
        //Move player based on keyboard input  
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");
        ammoText.text = "Ammo : " + ammo;
        healthText.text = "Health : " + health;
        moneyText.text = "Money : " + money; 
        controlPopupText.text = controlPopupMessage;

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
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            FireGun(angle);
            ammoText.text = "Ammo : " + ammo;
        }
		
		//damage color will fade
        if(damage != null)
        {
            if(damage.GetComponent<Image>().color.a > 0)
            {
                var color = damage.GetComponent<Image>().color;
                color.a -= 0.01f;

                damage.GetComponent<Image>().color = color;
            }
        }
    }

    void FireGun(float angle)
    {
        if (ammo > 0)
        {
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

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        healthText.text = "Health : " + health;

        if (otherGO.tag == "ProjectileEnemy")
        {
            Destroy(otherGO);
            health -= enemyProjectileDamage;
			gotHurt();
        }

        else if (otherGO.tag == "LockedDoor" && hasKey1 == false)
        {
            controlPopupMessage = "This door is locked";

        }
        else if (otherGO.tag == "FinalDoor" && hasKey2 == false)
        {
            controlPopupMessage = "This door is locked";

        }

        else if (otherGO.tag == "Enemy")
        {
            health -= 2;
            healthText.text = "Health : " + health;
			gotHurt();

        }
        else if (otherGO.tag == "CommandTerminal") {
            controlPopupMessage = "Press Space to Capture Ship";
        }
        else if (otherGO.tag == "Scrap") {
            controlPopupMessage = "Press Space to Pick Up Scrap";
        }
        else if (otherGO.tag == "Ammo") {
            controlPopupMessage = "Press Space to Pick Up Ammo";
        }
        else if (otherGO.tag == "Key") {
            controlPopupMessage = "Press Space to Pick Up Yellow Key Pad";
        }
        else if (otherGO.tag == "Key2") {
            controlPopupMessage = "Press Space to Pick Up Orange Key Pad";
        }

        if (health <= 0)
		{
			Destroy(gameObject);
            handleDeath.DeathLoadScene(livesRemaining);
            otherGO.GetComponent<Enemy>().playerSight = false;
		}

    }

    void OnCollisionExit(Collision coll) {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "CommandTerminal" || otherGO.tag == "Scrap" || otherGO.tag == "Ammo" || otherGO.tag == "Key" || otherGO.tag == "Key2" || otherGO.tag == "LockedDoor" || otherGO.tag == "FinalDoor") {
            controlPopupMessage = "";
        }
    }

    //Update this method to add player interactions with items
    void OnCollisionStay(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Ammo" && Input.GetKeyDown("space")) {
            Destroy(otherGO);
            controlPopupMessage = "";
            ammo += ammoPack;
        }
        else if (otherGO.tag == "Key" && Input.GetKeyDown("space")) {
            Destroy(otherGO);
            controlPopupMessage = "";
            hasKey1 = true;
            Key1();
        }
        else if (otherGO.tag == "Key2" && Input.GetKeyDown("space"))
        {
            Destroy(otherGO);
            controlPopupMessage = "";
            hasKey2 = true;
            Key2();
        }
        else if (otherGO.tag == "Scrap" && Input.GetKeyDown("space")) {
            Destroy(otherGO);
            controlPopupMessage = "";
            money += 100;
        }
        else if (otherGO.tag == "CommandTerminal" && Input.GetKeyDown("space")) {
            SceneManager.LoadScene("_Game_Win");
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Door")
        {
            if (other.GetComponent<Door>().Moving == false)
            {
                other.GetComponent<Door>().Moving = true;
            }
        }
        else if (other.tag == "LockedDoor" && hasKey1 == true)
        {
            if (other.GetComponent<Door>().Moving == false)
            {
                other.GetComponent<Door>().Moving = true;
            }
        }
        else if (other.tag == "FinalDoor" && hasKey2 == true)
        {
            if (other.GetComponent<Door>().Moving == false)
            {
                other.GetComponent<Door>().Moving = true;
            }
        }
    }
	
	//function for when the player takes damage to show red
    void gotHurt()
    {
        var color = damage.GetComponent<Image>().color;
        color.a = 0.8f;

        damage.GetComponent<Image>().color = color;

    }
    void Scrap()
    {
        var color = scrap.GetComponent<Image>().color;
        color.a = 1f;
        scrap.GetComponent<Image>().color = color;

    }

    void Key1()
    {
        var color = key1.GetComponent<Image>().color;
        color.a = 1f;
        key1.GetComponent<Image>().color = color;

    }
    void Key2()
    {
        var color = key2.GetComponent<Image>().color;
        color.a = 1f;
        key2.GetComponent<Image>().color = color;

    }
}