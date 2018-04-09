using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class JumpManager : MonoBehaviour {
	[SerializeField] private float _jumpTime = 0.5f;
	[SerializeField] private float _jumpLength = 10.0f;
	[SerializeField] private float _jumpHeight = 14.0f;
	[SerializeField] private AudioSource _regularJumpSound;

	private float _originalJumpHeight;
	private float _originalJumpLength;
	private float _originalJumpTime;

	private float _jumpStartPositionY;
	private float _jumpCounter = 0;
	private vThirdPersonController _controller;
	private bool _jumping = false;
	private bool _jumped = false;

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
		_controller.jumpForward = _jumpLength;
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
			StopJumping();
			RevertToOriginalSettings();
		}

		if (_jumping)
		{
			if (!_jumped)
			{
				_regularJumpSound.Play();
				_jumped = true;
			}
			_jumpCounter += Time.deltaTime;
			HandleJumpVelocity();

			if (Input.GetKeyUp(KeyCode.Space) && _jumpCounter < _jumpTime / 2)
			{
				_jumpCounter = _jumpTime / 2f;
				_jumpTime = _jumpTime * 1.2f;
			}
		}
		_controller.jumpForward = _jumpLength;
	}

	private void HandleJumpVelocity ()
	{
		Vector3 velocity = _controller._rigidbody.velocity;
		Vector3 position = _controller._rigidbody.position;
		// -2h/t^2
		float gravity = ( -2.0f * _jumpHeight ) / ( Mathf.Pow((_jumpTime / 2.0f), 2) );
		float initialVelocity = ( 2 * _jumpHeight ) / ( _jumpTime / 2.0f );
		//velocity.y = 0.5f * ( gravity * Mathf.Pow(_jumpCounter, 2) ) + initialVelocity * _jumpCounter;
		velocity.y = gravity * _jumpCounter + initialVelocity;
		//velocity.y = velocity.y * (_jumpTime / _jumpCounter);
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

	public void SetNormalJumpPadJump()
	{
		_jumpHeight = _originalJumpHeight * 2.5f;
		_jumpTime = _originalJumpTime * 1.5f;
	}

	public void SetSuperJumpPadJump()
	{
		_jumpHeight = _originalJumpHeight * 1.8f;
		_jumpTime = _originalJumpTime * 1.3f;
	}

	public void SetBubbleJump()
	{
		_jumpHeight = _originalJumpHeight * 1.6f;
		_jumpTime = _jumpTime * 1.6f;
	}

	public void StopJumping()
	{
		_jumped = false;
		_jumping = false;
		_jumpCounter = 0.0f;
	}

	public void ScaleJump(float scale)
	{
		RevertToOriginalSettings();
		_jumpHeight = _jumpHeight * scale;
		_jumpTime = _jumpTime * scale;
	}
}
