using Invector.CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour {
    
    private GameObject _player;
    private Collider _BoxCol;
    private CapsuleCollider _PlayerCol;
    private AudioSource _NoiseEffect;
    private bool _playerFacesToBox = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _PlayerCol = _player.GetComponent<CapsuleCollider>();
        _NoiseEffect = GetComponent<AudioSource>();
        _BoxCol = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update () {

        RaycastHit hit;
        Ray forward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, 1, 1)));

        Debug.DrawRay(_player.transform.position + new Vector3(0, 1, 0), _player.transform.TransformDirection(Vector3.forward)*1, Color.red);

        if (Physics.Raycast(forward, out hit, 2))
        {
            if (hit.collider == _BoxCol)
            {
                _playerFacesToBox = true;
            }
        }
        else
        {
            _playerFacesToBox = false;
        }

        if (Input.GetButton("Attack") && _playerFacesToBox)
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

        if (!Input.GetButton("Attack"))
        {
            transform.parent = null;
            Physics.IgnoreCollision(_BoxCol, _PlayerCol, false);
            _NoiseEffect.Stop();
            _player.GetComponent<vThirdPersonController>().isStrafing = false;
            _player.GetComponent<vThirdPersonController>().strafeRotationSpeed = 10;
            _player.GetComponent<Animator>().SetBool("pushing", false);
        }
    }
}