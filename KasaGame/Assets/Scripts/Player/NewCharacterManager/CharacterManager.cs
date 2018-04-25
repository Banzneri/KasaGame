using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
	private JumpManager _jumpManager;
	private MoveManager _moveManager;
	private InputManager _inputManager;
	private AnimManager _animManager;
	private WeaponManager _weaponManager;

	void Start () {
		_jumpManager = GetComponent<JumpManager>();
		_moveManager = GetComponent<MoveManager>();
		_inputManager = GetComponent<InputManager>();
		_animManager = GetComponent<AnimManager>();
		_weaponManager = GetComponent<WeaponManager>();
	}
	
	void Update () {
		
	}
}
