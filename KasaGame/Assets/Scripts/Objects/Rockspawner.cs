using Invector.CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockspawner : MonoBehaviour {
    public float summonTime = 400;
    public float delay = 0;
    private float _timeCounter = 0;
    public GameObject rock;


	// Use this for initialization
	void Start () {
        _timeCounter = summonTime - delay;
    }
	
	// Update is called once per frame
	void Update () {
        if(_timeCounter < summonTime)
        {
            _timeCounter++;
        }
        else
        {
            Instantiate(rock, transform.position, transform.rotation);
            _timeCounter = 0;
        }
    }
}
