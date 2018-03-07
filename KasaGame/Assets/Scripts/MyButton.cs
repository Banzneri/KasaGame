using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour, ITriggerObject<IActionObject> {
    [SerializeField]
    public GameObject[] actionObjects;
    private bool inTrigger;
    private Animator anim;
    private new AudioSource audio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    private void Update()
    {
        if (inTrigger)
        {
            if(Input.GetButtonDown("action"))
            {
                audio.Play();
                anim.Play("ButtonPress");
                for(int i = 0; i < actionObjects.Length; i++)
                {
                    Trigger(actionObjects[i].GetComponent<IActionObject>());
                }
            }
        }
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    private void OnGUI()
    {
        if (inTrigger)
        {
                GUI.Box(new Rect(450, 400, 200, 25), "Press E to interact");
        }
    }

    public void TriggerAll()
    {
        
    }
}
