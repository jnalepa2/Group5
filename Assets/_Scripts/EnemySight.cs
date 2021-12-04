using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
	public bool playerIsIn;
    // Start is called before the first frame update
    void Start()
    {
        playerIsIn = false;
    }

    void OnTriggerEnter( Collider coll )
	{
		GameObject collidedWith = coll.gameObject;
		
		if (collidedWith.tag == "Player")
			playerIsIn = true;
		
	}
	
	void OnTriggerExit( Collider coll )
	{
		GameObject collidedWith = coll.gameObject;
		
		if (collidedWith.tag == "Player")
			playerIsIn = false;
		
	}
	
	
	public bool IsIn(){
		return playerIsIn;
	}
}
