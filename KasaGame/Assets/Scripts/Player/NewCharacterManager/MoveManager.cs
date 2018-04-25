using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour {
	private bool _canMove = true;
	private float _speed = 0.0f;

	private Rigidbody _rb;
	private InputManager _inputManager;

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
		_inputManager = GetComponent<InputManager>();
	}
	
	void Update () 
	{
		UpdateMovement();
	}

	private void UpdateMovement ()
	{

	}
}
