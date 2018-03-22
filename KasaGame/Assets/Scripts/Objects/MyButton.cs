using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour, ITriggerObject<IActionObject> {
    [SerializeField]
    public GameObject[] actionObjects;
    private Animator anim;
    private new AudioSource audio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    public void TriggerAll()
    {
        audio.Play();
        anim.Play("ButtonPress");
        for(int i = 0; i < actionObjects.Length; i++)
        {
            Trigger(actionObjects[i].GetComponent<IActionObject>());
        }
        Debug.Log("Trigger");
    }
}
