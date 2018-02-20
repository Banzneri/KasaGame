using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVer2 : MonoBehaviour, IActionObject
{
    public Transform[] moveLocations;
    public float speed;
    public bool automatic;
    public bool loop;
    private bool backwardLoop = false;
    private Vector3 currentDestination;

    // Use this for initialization
    void Start()
    {
        currentDestination = new Vector3(moveLocations[0].position.x, moveLocations[0].position.y, moveLocations[0].position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 trajectory = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);
        transform.position = trajectory;

        if(automatic)
        {
            Action();
        }  
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent = transform;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent = null;
        }
    }

    public void Action()
    {
        if(loop)
        {
            for (int i = 0; i < moveLocations.Length; i++)
            {
                if (transform.position == moveLocations[i].position)
                {
                    if (i + 1 < moveLocations.Length)
                    {
                        currentDestination = moveLocations[i + 1].transform.position;
                    }
                    else
                    {
                        currentDestination = moveLocations[0].transform.position;
                    }
                    GetComponent<AudioSource>().Play();
                }
            }
        }
        else if(!loop)
        {
            for (int i = 0; i < moveLocations.Length; i++)
            {
                if (transform.position == moveLocations[i].position && !backwardLoop)
                {
                    if (i + 1 < moveLocations.Length)
                    {
                        currentDestination = moveLocations[i + 1].transform.position;
                    }
                    else
                    {
                        currentDestination = moveLocations[i-1].transform.position;
                        backwardLoop = true;
                    }
                        GetComponent<AudioSource>().Play();
                }
                else if (transform.position == moveLocations[i].position && backwardLoop)
                {
                    if (i - 1 >= 0)
                    {
                        currentDestination = moveLocations[i - 1].transform.position;
                    }
                    else
                    {
                        currentDestination = moveLocations[i + 1].transform.position;
                        backwardLoop = false;
                    }
                        GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
