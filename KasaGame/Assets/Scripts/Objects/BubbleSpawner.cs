using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour, IActionObject {
    public float summonTime = 400;
    public float delay = 0;
    private float _timeCounter = 0;
    public Bubble bubble;
    private AudioSource _popSound;
    public bool activated = true;


	// Use this for initialization
	void Start () {
        _timeCounter = summonTime - delay;
        _popSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if(_timeCounter < summonTime && activated)
        {
            _timeCounter++;
        }
        else if (activated)
        {
            bubble.originSpawner = this;
            Instantiate(bubble, transform.position, transform.rotation);
            _timeCounter = 0;
        }
	}

    public void PlayEffect()
    {
        _popSound.Play();
    }

    public void Action()
    {
        if(activated == false)
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
        
    }
}
