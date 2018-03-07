using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSwitch : MonoBehaviour, ITriggerObject<IActionObject>
{
    [SerializeField]
    public GameObject[] actionObjects;
    private bool inTrigger;
    private bool activated = false;
    public float speedMultiplier = 0.5f;
    private Animator anim;
    private new AudioSource audio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("speed", speedMultiplier);
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
        if (inTrigger && !activated)
        {
            if (Input.GetButtonDown("action"))
            {
                for (int i = 0; i < actionObjects.Length; i++)
                {
                    Trigger(actionObjects[i].GetComponent<IActionObject>());
                }
                activated = true;
                anim.Play("GoDown");
                audio.Play();
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("EndAction") && activated)
        {
            audio.Stop();
                for (int i = 0; i < actionObjects.Length; i++)
                {
                    Trigger(actionObjects[i].GetComponent<IActionObject>());
                }
                activated = false;
        }
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    private void OnGUI()
    {
        if (inTrigger && !activated)
        {
            GUI.Box(new Rect(450, 400, 200, 25), "Press E to interact");
        }
    }
}
