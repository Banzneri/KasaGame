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

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("EndAction") && activated)
        {
            audio.Stop();
            activated = false;
            for (int i = 0; i < actionObjects.Length; i++)
            {
                Trigger(actionObjects[i].GetComponent<IActionObject>());
            }
        }
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    public void TriggerAll()
    {
        if (!activated)
        {
            activated = true;
            anim.Play("GoDown");
            audio.Play();
            for (int i = 0; i < actionObjects.Length; i++)
            {
                Trigger(actionObjects[i].GetComponent<IActionObject>());
            }
        }
    }
}
