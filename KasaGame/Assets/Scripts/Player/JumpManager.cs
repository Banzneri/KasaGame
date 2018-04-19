using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class JumpManager : MonoBehaviour {
	[SerializeField] private float _jumpTime = 0.5f;
	[SerializeField] private float _jumpLength = 10.0f;
	[SerializeField] private float _jumpHeight = 14.0f;
	[SerializeField] private AudioSource _regularJumpSound;
	[SerializeField] private float _fallCooldown = 0.2f;
	[SerializeField] private float _jumpHeightFromGround = 1f;
	[SerializeField] private Projector _blobShadow;

	private float _originalJumpHeight;
	private float _originalJumpLength;
	private float _originalJumpTime;
	private vThirdPersonController _controller;

	private float _minJumpTime = 0.1f;
	private float _startVelocity = 0.0f;
	private bool _reachedApex = false;
	private bool _pressedJumpKey = false;
	private bool _releasedJumpKey = false;
	private bool _releasedEarly = false;
	private bool _alreadyReleased = false;
	private bool _wantsToJump = false;
	private bool _wantsToReleaseJumpKey = false;
	private float _jumpStartPositionY;
	private float _jumpCounter = 0.0f;
	private float _variableJumpCounter = 0.0f;
	private bool _jumping = false;
	private bool _jumped = false;
	private bool _startedFalling = false;
	private float _fallCounter = 0.0f;
	private bool _alreadyFell = true;
	private float _timeInApex = 8.0f;
	private float _timeInApexCounter = 0.0f;

	public bool _isRegularJump = true;
	public bool _jumpedWhileFalling = false;

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

	public bool CanJump
	{
		get { return _startedFalling || _controller.GroundDistance < _jumpHeightFromGround; }
	}

	void Start () 
	{
		_controller = GetComponent<vThirdPersonController>();
		_originalJumpHeight = _jumpHeight;
		_originalJumpLength = _jumpLength;
		_originalJumpTime = _jumpTime;
		_controller.jumpForward = _jumpLength;
	}
	
	void FixedUpdate () 
	{
		HandleJumpVelocity();
	}

	void Update ()
	{
		HandleJumping();
		HandleVariableHeightJump();
		HandleFalling();
		GetInput();
	}

	public void GetInput()
	{
		_pressedJumpKey = Input.GetKeyDown(KeyCode.Space);
		_releasedJumpKey = Input.GetKeyUp(KeyCode.Space);

		if (!_wantsToReleaseJumpKey && _releasedJumpKey)
		{
			_wantsToReleaseJumpKey = true;
		}

		if (!_wantsToJump && _pressedJumpKey && CanJump)
		{
			_wantsToJump = true;	
		}
	}

	public void StartJumping(bool isRegularJump)
	{
		StopJumping();
		DisableRealShadows();
		_isRegularJump = isRegularJump;
		if (_isRegularJump) _regularJumpSound.Play();
		_jumping = true;
		_jumpStartPositionY = _controller._rigidbody.position.y;
		if (_controller._rigidbody.velocity.magnitude < 1)
			GetComponent<Animator>().CrossFadeInFixedTime("Jump", 0.01f);
		else
			GetComponent<Animator>().CrossFadeInFixedTime("JumpMove", 0.1f);
	}

	public void StopJumping()
	{
		EnableRealShadows();
		RevertToOriginalSettings();
		_jumped = false;
		_jumping = false;
		_jumpCounter = 0.0f;
		_variableJumpCounter = 0.0f;
		_timeInApexCounter = 0.0f;
		_reachedApex = false;
		_wantsToJump = false;
		_alreadyReleased = false;
		_wantsToReleaseJumpKey = false;
		_releasedEarly = false;
	}

	private void HandleJumping()
	{
		if (_controller.isGrounded && _wantsToJump)
		{
			NormalJump();
		}
		else if (_controller.isGrounded && _jumpCounter > _jumpTime / 2)
		{
			StopJumping();
		}
		_controller.jumpForward = _jumpLength;
	}

	private void HandleFalling()
	{
		GetComponent<Animator>().SetBool("IsFalling", !CanJump);
		if (Jumping  || _controller.isGrounded)
		{
			StopFalling();
			_alreadyFell = true;
			return;
		}

		if (!_startedFalling && _alreadyFell && !_controller.isGrounded && !Jumping)
		{
			_startedFalling = true;
		}

		if (_startedFalling)
		{
			_fallCounter += Time.deltaTime;
			Debug.Log(_fallCounter);
			if (_wantsToJump) NormalJump();
			if (_fallCounter > _fallCooldown) StopFalling();
		}
	}

	private void StopFalling()
	{
		_fallCounter = 0.0f;
		_startedFalling = false;
		_alreadyFell = false;
	}

	private void HandleVariableHeightJump()
	{
		if (!_isRegularJump || !Jumping || _alreadyReleased) return;
		
		_variableJumpCounter += Time.deltaTime;

		if (_wantsToReleaseJumpKey && _variableJumpCounter < _minJumpTime)
		{
			_releasedEarly = true;
		}

		if (_releasedEarly && _variableJumpCounter > _minJumpTime)
		{
			_alreadyReleased = true;
		}
		else if (!_releasedEarly && _wantsToReleaseJumpKey && _variableJumpCounter < _jumpTime / 2)
		{
			_alreadyReleased = true;
		}

		if (_alreadyReleased)
		{
			_jumpCounter = _jumpTime * 0.45f;
			_jumpTime = _jumpTime * 1.1f;
		}
	}

	private void HandleJumpVelocity ()
	{
		if (!_jumping) return;

		_jumpCounter += Time.fixedDeltaTime;
		Vector3 velocity = _controller._rigidbody.velocity;
		float gravity = ( -2.0f * _jumpHeight ) / ( Mathf.Pow((_jumpTime / 2.0f), 2) );
		float initialVelocity = ( 2 * _jumpHeight ) / ( _jumpTime / 2.0f );
		velocity.y = gravity * _jumpCounter + initialVelocity;

		if (!_reachedApex && velocity.y <= 5)
		{
			_reachedApex = true;
			_alreadyReleased = true;
			if (_isRegularJump)
			{
				_jumpTime = _jumpTime * 1.1f;
			}
		}

		if (_jumpCounter > _jumpTime * 0.66f && _isRegularJump && _reachedApex)
		{
			_jumpTime = _originalJumpTime;
		}
		_controller._rigidbody.velocity = velocity;
	}

	public void RevertToOriginalSettings()
	{
		_jumpHeight = _originalJumpHeight;
		_jumpLength = _originalJumpLength;
		_jumpTime = _originalJumpTime;
	}

	public void NormalJump()
	{
		StartJumping(true);
		Debug.Log("NormalJump");
		_jumpHeight = _originalJumpHeight;
		_jumpLength = _originalJumpLength;
		_jumpTime = _originalJumpTime;
	}

	public void NormalJumpPadJump()
	{
		StartJumping(false);
		_jumpHeight = _originalJumpHeight * 1.8f;
		_jumpTime = _originalJumpTime * 1.3f;
	}

	public void SuperJumpPadJump()
	{
		StartJumping(false);
		_jumpHeight = _originalJumpHeight * 2.5f;
		_jumpTime = _originalJumpTime * 1.5f;
	}

	public void EdgeJump()
	{
		StartJumping(false);
		_jumpHeight = _originalJumpHeight * 0.5f;
	}

	public void BubbleJump()
	{
		StartJumping(false);
		_jumpHeight = _originalJumpHeight * 1.6f;
		_jumpTime = _originalJumpTime * 1.6f;
	}

	public void EnableRealShadows()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
		_blobShadow.enabled = false;
	}

	public void DisableRealShadows()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
		_blobShadow.enabled = true;
	}

	private void JumpDebug()
	{
		Debug.Log("AlreadyFell: " + _alreadyFell);
		Debug.Log("VariableCounter: " + _variableJumpCounter);
		Debug.Log("Jumping: " + _jumping);
		Debug.Log("AlreadyReleased: " + _alreadyReleased);
		Debug.Log("ReleasedEarly: " + _releasedEarly);
		Debug.Log("JumpCounter: " + _jumpCounter);
	}
}
