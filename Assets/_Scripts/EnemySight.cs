using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
	private bool playerIsIn;
    // Start is called before the first frame update
    void Start()
    {
        playerIsIn = false;
    }

    void OnCollisionEnter( Collision coll )
	{
		GameObject collidedWith = coll.gameObject;
		
		if (collidedWith.tag == "Player")
			playerIsIn = true;
		
	}
	
	void OnCollisionExit( Collision coll )
	{
		GameObject collidedWith = coll.gameObject;
		
		if (collidedWith.tag == "Player")
			playerIsIn = false;
		
	}
	
	
	public bool IsIn(){
		return playerIsIn;
	}
}
