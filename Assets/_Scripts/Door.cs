using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 endPos;
    public float speed = 1.0f;

    private bool moving = false;
    private bool opening = true;
    private Vector3 startPos;
    private float delay = 0.0f;
    public state doorState = state.closed;

    public enum state
    {
        closing,
        opening,
        closed,
        open
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (opening)
            {
                moveDoor(endPos);
            }
            else
            {
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
            if (delay < 3.0f)
            {
                delay += Time.deltaTime;
                if (opening)
                {
                    opening = false;
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
    

    public bool isClosed()
    {
        return doorState == state.closed;
    }

    public bool isOpen()
    {
        return doorState == state.open;
    }

}
