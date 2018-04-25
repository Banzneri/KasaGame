using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	private CharacterManager _characterManager;

	[SerializeField] private string _horizontalInput = "Horizontal";
	[SerializeField] private string _verticalInput = "Vertical";
	[SerializeField] private string _horizontalCameraInput = "Mouse X";
	[SerializeField] private string _verticalCameraInput = "Mouse Y";

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _cameraInput = Vector2.zero;
	private bool _canUseInput = true;

	public bool Jump 
	{
		get { return Input.GetButtonDown("Jump"); }
	}

	public bool ReleaseJump
	{
		get { return Input.GetButtonUp("Jump"); }
	}

	public bool Attack
	{
		get { return Input.GetButtonDown("Attack"); }
	}

	public bool Throw
	{
		get { return Input.GetButtonDown("Throw"); }
	}

	public bool PauseMenu
	{
		get { return Input.GetButtonDown("Pause"); }
	}

	public bool CanUseInput
	{
		get { return _canUseInput; }
		set { _canUseInput = value; }
	}

	void Start () {
		_characterManager = GetComponent<CharacterManager>();
	}
	
	void Update () {
		UpdateMoveInput();
		UpdateCameraInput();
	}

	private void UpdateMoveInput()
	{
		_moveInput.x = Input.GetAxis(_horizontalInput);
		_moveInput.y = Input.GetAxis(_verticalInput);
	}

	private void UpdateCameraInput()
	{
		_cameraInput.x = Input.GetAxis(_horizontalCameraInput);
		_cameraInput.y = Input.GetAxis(_verticalCameraInput);
	}
}
