using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOut : MonoBehaviour, ITriggerObject<IActionObject>
{
    private List<LightsOutCube> _cubes = new List<LightsOutCube>();
    private bool _unlocked = false;
    public int activeCubes = 0;
    public GameObject[] actionObjects;
    private AudioSource _successSound;

    private void Start()
    {
        _successSound = GetComponent<AudioSource>();
        foreach (Transform trans in gameObject.transform)
        {
            _cubes.Add(trans.gameObject.GetComponentInChildren<LightsOutCube>());
            if (trans.gameObject.GetComponentInChildren<LightsOutCube>().activated == true)
            {
                activeCubes++;
            }
        }
        Debug.Log(_cubes.Count);
    }

    private void Update()
    {

        if (activeCubes == _cubes.Count && _unlocked == false)
        {
            _unlocked = true;
            TriggerAll();
            _successSound.Play();
        }
        else if(activeCubes != _cubes.Count && _unlocked == true)
        {
            _unlocked = false;
            TriggerAll();
        }
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    public void TriggerAll()
    {
        for (int i = 0; i < actionObjects.Length; i++)
        {
            actionObjects[i].GetComponent<IActionObject>().Action();
        }
    }
}
