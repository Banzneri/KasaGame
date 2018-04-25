using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class JumpManager2 : MonoBehaviour {

	[SerializeField] private float _jumpStartVelocity = 50f;
	[SerializeField] private float _gravity = 2f;
	[SerializeField] private float _jumpHeightFromGround = 0.2f;

	private bool _jumping = false;
	private bool _releasedJump = false;
	private bool _releasedEarly = false;
	private float _jumpCounter = 0.0f;
	private float _jumpTime = 0.8f;
	private float _variableJumpCounter = 0.0f;
	private float _minJumpTime = 0.1f;
	private float _velocityY = 0.0f;
	private Rigidbody _rb;
	private vThirdPersonController _controller;

	public bool CanJump
	{
		get { return _controller.GroundDistance < _jumpHeightFromGround; }
	}

	public bool Jumping
	{
		get { return _jumping; }
	}

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
		_controller = GetComponent<vThirdPersonController>();
	}

	void FixedUpdate ()
	{
		HandleJumpVelocity();
	}
	
	void Update () 
	{
		GetInput();
	}

	private void GetInput ()
	{
		if (Input.GetKeyDown(KeyCode.Space) && CanJump)
		{
			StartJumping();
		}

		_releasedJump = Input.GetKeyUp(KeyCode.Space);
	}

	private void HandleJumpVelocity ()
	{
		if (!_jumping) return;

		_jumpCounter += Time.fixedDeltaTime;
		_velocityY -= _gravity;
		Vector3 velocity = _rb.velocity;
		velocity.y = _velocityY;
		_rb.velocity = velocity;

		if (_controller.isGrounded && _jumpCounter > _jumpTime / 2)
		{
			StopJumping();
		}
	}

	private void StartJumping ()
	{
		_jumping = true;
		_velocityY = _jumpStartVelocity;
	}

	private void StopJumping ()
	{
		_jumping = false;
		_jumpCounter = 0.0f;
		_variableJumpCounter = 0.0f;
		_velocityY = 0.0f;
	}

	private void HandleVariableJumping ()
	{
		if (!_jumping)
		{
			return;
		}

		_variableJumpCounter += Time.deltaTime;

		if (!(_velocityY < 0) && _releasedJump)
		{
			_releasedEarly = true;
		}
	}
}
