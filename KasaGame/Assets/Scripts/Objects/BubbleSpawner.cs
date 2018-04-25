using Invector.CharacterController;
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
    public float originalPlayerJumpHeight;
    private GameObject _player;


	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
        originalPlayerJumpHeight = _player.GetComponent<JumpManager>().JumpHeight;
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

        //Change the jump height back to normal when appropriate
        if (_player.GetComponent<vThirdPersonController>().jumpCounter == 0 ||
            _player.GetComponent<vThirdPersonController>().isGrounded)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = originalPlayerJumpHeight;
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
