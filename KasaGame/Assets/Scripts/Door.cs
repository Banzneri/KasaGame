using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool doorKey;
    public bool open;
    public bool close;
    public bool inTrigger;



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
            if(close) //if door is closed, check if it should be opened
            {
                    if(Input.GetButtonDown("action") && doorKey)
                    {
                        open = true;
                        close = false;
                    }
            }
            else //if door is open, check if it should be closed
            {
                if (Input.GetButtonDown("action"))
                {
                    open = false;
                    close = true;
                }
            }
        }
            if(open) //opens the door, if it should be open
            {
                Quaternion doorTurn = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), Time.deltaTime * 200);
                transform.parent.transform.rotation = doorTurn;
            }
            else //closes the door, if it should be closed
            {
                Quaternion doorTurn = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), Time.deltaTime * 200);
                transform.parent.transform.rotation = doorTurn;
            }
    }

    private void OnGUI()
    {
        if(inTrigger)
        {
            if(open)
            {
                GUI.Box(new Rect(450, 400, 200, 25), "Press E to close");
            }
            else
            {
                if(doorKey)
                {
                    GUI.Box(new Rect(450, 400, 200, 25), "Press E to open");
                }
                else
                {
                    GUI.Box(new Rect(450, 400, 200, 25), "Need a key");
                }
            }
        }
    }
}
