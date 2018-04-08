using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class JumpManager : MonoBehaviour {
	[SerializeField] private float _jumpTime = 0.5f;
	[SerializeField] private float _jumpLength = 10.0f;
	[SerializeField] private float _jumpHeight = 14.0f;

	private float _originalJumpHeight;
	private float _originalJumpLength;
	private float _originalJumpTime;

	private float _jumpStartPositionY;
	private float _jumpCounter = 0;
	private vThirdPersonController _controller;
	private bool _jumping = false;

	public float JumpHeight
	{
		get { return _jumpHeight; }
		set { _jumpHeight = value; }
	}

	public float JumpLength
	{
		get { return _jumpLength; }
		set { _jumpLength = value; }
	}

	public float JumpTime 
	{
		get { return _jumpTime; }
		set { _jumpTime = value; }
	}

	public bool Jumping
	{
		get { return _jumping; }
		set { _jumping = value; }
	}

	void Start () 
	{
		_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonController>();
		_originalJumpHeight = _jumpHeight;
		_originalJumpLength = _jumpLength;
		_originalJumpTime = _jumpTime;
	}
	
	void LateUpdate () 
	{
		HandleJumping();
	}

	private void HandleJumping()
	{
		if (_controller.isJumping && !_jumping)
		{
			_jumpStartPositionY = _controller._rigidbody.position.y;
			_jumping = true;
		}

		else if (_controller.isGrounded && !_controller.isJumping && _jumping)
		{
			_jumping = false;
			_jumpCounter = 0.0f;
			RevertToOriginalSettings();
			Vector3 vel = _controller._rigidbody.velocity;
			vel.y = 0.0f;
			_controller._rigidbody.velocity = vel;
		}

		if (_jumping)
		{
			_jumpCounter += Time.deltaTime;
			HandleJumpVelocity();
		}
	}

	private void HandleJumpVelocity ()
	{
		Vector3 velocity = _controller._rigidbody.velocity;
		Vector3 position = _controller.GetComponent<Rigidbody>().position;
		// -2h/t^2
		float gravity = ( -2.0f * _jumpHeight ) / ( Mathf.Pow((_jumpTime / 2.0f), 2) );
		float initialVelocity = ( 2 * _jumpHeight ) / ( _jumpTime / 2.0f );
		velocity.y = 0.5f * ( gravity * Mathf.Pow(_jumpCounter, 2) ) + initialVelocity * _jumpCounter;
		velocity.y = velocity.y * (_jumpTime / _jumpCounter);
		//if (velocity.y < -30) velocity.y = -30;
		Debug.Log(velocity.y);
		_controller._rigidbody.velocity = velocity;
	}

	public void RevertToOriginalSettings()
	{
		_jumpHeight = _originalJumpHeight;
		_jumpLength = _originalJumpLength;
		_jumpTime = _originalJumpTime;
	}

	public void StopJumping()
	{
		_jumping = false;
		_jumpCounter = 0.0f;
	}

	public void ScaleJump(float scale)
	{
		RevertToOriginalSettings();
		_jumpHeight = _jumpHeight * scale;
		_jumpLength = _jumpLength * scale;
		_jumpTime = _jumpTime * scale;
	}
}
