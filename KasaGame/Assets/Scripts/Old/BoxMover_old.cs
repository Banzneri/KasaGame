using Invector.CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover_old : MonoBehaviour {
    
    private GameObject _player;
    private Rigidbody _RBody;
    private BoxCollider _BoxCol;
    private CapsuleCollider _PlayerCol;
    private AudioSource _NoiseEffect;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _RBody = GetComponent<Rigidbody>();
        _BoxCol = GetComponent<BoxCollider>();
        _PlayerCol = _player.GetComponent<CapsuleCollider>();
        _NoiseEffect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

        float distanceToPlayer = Vector3.Distance(this.transform.position, _player.transform.position);

        //Debug.Log(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"));


        if (Input.GetButton("action") && distanceToPlayer < 2.5)
        {
            transform.SetParent(_player.transform);
            _player.GetComponent<vThirdPersonController>().isStrafing = true;
            _player.GetComponent<vThirdPersonController>().strafeRotationSpeed = 0;
            Physics.IgnoreCollision(_BoxCol, _PlayerCol, true);

            if ((Math.Abs(Input.GetAxis("Horizontal")) > 0.5 || Math.Abs(Input.GetAxis("Vertical")) > 0.5) && !_NoiseEffect.isPlaying)
            {
                _NoiseEffect.Play();
            }
            else if ((Math.Abs(Input.GetAxis("Horizontal")) < 0.5 && Math.Abs(Input.GetAxis("Vertical")) < 0.5))
            {
                _NoiseEffect.Stop();
            }

            _player.GetComponent<Animator>().SetBool("pushing", true);
        }
        else
        {
            transform.parent = null;
            _player.GetComponent<vThirdPersonController>().isStrafing = false;
            _player.GetComponent<vThirdPersonController>().strafeRotationSpeed = 10;
            Physics.IgnoreCollision(_BoxCol, _PlayerCol, false);
            _NoiseEffect.Stop();
            _player.GetComponent<Animator>().SetBool("pushing", false);
        }
    }
}