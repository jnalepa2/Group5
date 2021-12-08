using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
	public AudioSource self;
	
	void Awake()
	{
		self = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        if(!self.isPlaying)		Destroy(gameObject);
    }
}
