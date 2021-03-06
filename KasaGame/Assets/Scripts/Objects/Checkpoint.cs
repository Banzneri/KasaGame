﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	[SerializeField] private Material _activeMaterial;
	[SerializeField] private Material _inactiveMaterial;
	[SerializeField] private bool _isActivated = false;
	[SerializeField] private GameObject[] _sides;
	[SerializeField] private GameObject _gear;
	[SerializeField] private float _actDistance = 5.0f;

	private GameObject _player;
	private bool _hasBeenActivated = false;

	bool WithinDistance {
		get {
			return Vector3.Distance(transform.position, _player.transform.position) < _actDistance;
		}
	}

	void Start () {
		SetMaterials();
		_player = GameObject.FindGameObjectWithTag("Player");
		_isActivated = _gear.GetComponent<RotateGear>().isActivated;
	}
	
	void Update () {
		if (!_hasBeenActivated && _isActivated)
		{
			Activate();
		}
		Interact ();
	}

	void SetMaterials ()
	{
		foreach (var side in _sides)
		{
			side.GetComponent<MeshRenderer>().material = _isActivated ? _activeMaterial : _inactiveMaterial;
		}
	}

	void Interact()
    {
        if (WithinDistance)
        {
            if (!_isActivated)
            {
                Activate();
            }
        }
    }

	void Activate ()
	{
		_gear.GetComponent<RotateGear>().Activate();
		_gear.GetComponent<AudioSource>().Play();
		_isActivated = true;
		SetMaterials();
		_hasBeenActivated = true;
	}
}
