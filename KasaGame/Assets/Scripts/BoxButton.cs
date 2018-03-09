using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButton : MonoBehaviour, ITriggerObject<IActionObject> {

    public GameObject[] actionObjects;
    private AudioSource _soundEffect;

    private void Start()
    {
        _soundEffect = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            _soundEffect.Play();
            for (int i = 0; i < actionObjects.Length; i++)
            {
                Trigger(actionObjects[i].GetComponent<IActionObject>());
            }
            transform.position -= new Vector3(0, 0.15f, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            _soundEffect.Play();
            for (int i = 0; i < actionObjects.Length; i++)
            {
                Trigger(actionObjects[i].GetComponent<IActionObject>());
            }
            transform.position += new Vector3(0, 0.15f, 0);
        }
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    public void TriggerAll()
    {
        throw new NotImplementedException();
    }
}
