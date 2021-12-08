using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 endPos;
    public float speed = 1.0f;
	public AudioSource openingSound;
	public AudioSource closingSound;

    private bool moving = false;
    private bool opening = true;
    private Vector3 startPos;
    private float delay = 0.0f;
	
	private bool doOpen = true;
	private bool doClose = true;

    // Start is called before the first frame update
    void Start()
    {
		openingSound = Instantiate<AudioSource>(openingSound);
		closingSound = Instantiate<AudioSource>(closingSound);
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (opening)
            {
				if(doOpen){
					openingSound.Play();
					doOpen = false;
				}
                moveDoor(endPos);
            }
            else
            {
				if(doClose){
					closingSound.Play();
					doClose = false;
				}
                moveDoor(startPos);
            }
        }
		
    }

    void moveDoor(Vector3 goalPos)
    {
        float dist = Vector3.Distance(transform.position, goalPos);

        if (dist > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, goalPos, speed * Time.deltaTime);
        }
        else
        {
            if (opening)
            {
                delay += Time.deltaTime;
                if (delay > 1.5f)
                {
                    opening = false;
					doOpen = true;
					doClose = true;
                }
            }
            else
            {
                moving = false;
                opening = true;
            }
        }

    }

    public bool Moving
    {
        get { return moving; }
        set { moving = value; }
    }

}
