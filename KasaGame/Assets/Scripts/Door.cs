using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActionObject {

    public bool doorKey;
    public bool open;

    public int dir = 1;
    private bool inTrigger;
    public float direction;


    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    void Update()
    {
        if(inTrigger)  //if player is close enough, check for action
        {
            if(Input.GetButtonDown("action") && doorKey)
            {
                Action();
                doorKey = false;
            }
        }

        if (open) //opens the door, if it should be open
        {
            Quaternion doorTurn = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, -90f - direction, 0.0f), Time.deltaTime * 200);
            transform.rotation = doorTurn;
        }
        else //closes the door, if it should be closed
        {
            Quaternion doorTurn = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f + direction, 0.0f), Time.deltaTime * 200);
            transform.rotation = doorTurn;
        }
    }

    private void OnGUI()
    {
        if(inTrigger)
        {
            if(open && doorKey)
            {
                GUI.Box(new Rect(450, 400, 200, 25), "Press E to close");
            }
            else if(!open & doorKey)
            {
                GUI.Box(new Rect(450, 400, 200, 25), "Press E to open");
            }
            else if (!open & !doorKey)
            {
                GUI.Box(new Rect(450, 400, 200, 25), "Can't open");
            }
            else if (open & !doorKey)
            {
                GUI.Box(new Rect(450, 400, 200, 25), "Can't close");
            }
        }
    }

    public void Action()
    {
        if (open)
        {
            open = false;
        }
        else
        {
            open = true;
        }
    }
}
