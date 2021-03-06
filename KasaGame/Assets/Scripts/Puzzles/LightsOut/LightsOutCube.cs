﻿using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOutCube : MonoBehaviour {
    public LightsOutCube[] adjacents;
    public bool activated;
    public Material _inactiveMaterial;
    public Material _activeMaterial;
    private Light _light;
    private MeshRenderer _renderer;
    private GameObject _player;
    private bool _playerOnTop;
    private bool _playerJustEntered = false;
    public AudioSource _soundOn;
    public AudioSource _soundOff;
    private LightsOut _manager;
    private float _originalJumpHeight;

    private void Start()
    {
        _manager = gameObject.GetComponentInParent<LightsOut>();
        _light = GetComponent<Light>();
        _renderer = GetComponent<MeshRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (activated)
        {
            _light.enabled = true;
            _renderer.material = _activeMaterial;
        }
        else
        {
            _light.enabled = false;
            _renderer.material = _inactiveMaterial;
        }
    }


    public void Change()
    {
        if (activated)
        {
            if (!_soundOff.isPlaying)
            {
                _soundOff.Play();
            }
            activated = false;
            _light.enabled = false;
            _renderer.material = _inactiveMaterial;
            _manager.activeCubes--;
        }
        else
        {
            if (!_soundOn.isPlaying)
            {
                _soundOn.Play();
            }
            activated = true;
            _light.enabled = true;
            _renderer.material = _activeMaterial;
            _manager.activeCubes++;
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)));

        Debug.DrawRay(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)), Color.red);

        if (Physics.Raycast(downward, out hit, 1))
        {
            if (hit.collider == transform.GetComponent<BoxCollider>() && 
                _player.GetComponent<Rigidbody>().velocity.y < -1 &&
                _player.GetComponent<JumpManager>().Jumping)
            {
                _playerOnTop = true;
            }
        }
        else
        {
            _playerOnTop = false;
            _playerJustEntered = false;
        }

        if (_playerOnTop == true && _playerJustEntered == false && !_manager.Completed)
        {
            _playerJustEntered = true;
            Change();
            for (int i = 0; i < adjacents.Length; i++)
            {
                adjacents[i].Change();
            }
        }
    }
}
