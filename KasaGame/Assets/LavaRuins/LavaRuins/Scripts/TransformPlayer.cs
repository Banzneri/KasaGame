using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class TransformPlayer : MonoBehaviour {

	#region Public Variables

	[SerializeField]
	private vThirdPersonController _VController;

	[SerializeField]
	private Rigidbody _Rigidbody;

	[SerializeField]
	private GameObject _Plate;

	[Header("Settings")]

	// List of targets
	[SerializeField]
	private List<Transform> _Targets;

	// Move speed
	[SerializeField]
	private float _MoveSpeed;

	#endregion

	#region Private Variables

	// Is player being moved
	private bool _Moving;

	// Is player transformed
	private bool _Transformed;

	// Current destination of platform
	private Vector3 _CurrentDestination;

	// Index of current destination
	private int _Index;

	// Player character
	private GameObject _Player;

	#endregion

	void Start() {
		_Moving = false;
		_Transformed = false;
		_Index = 0;
		_CurrentDestination = _Targets [_Index].position;
	}

	void FixedUpdate() {

		if (_Moving) {

			Vector3 trajectory = Vector3.MoveTowards(_Plate.transform.position, _CurrentDestination, _MoveSpeed * Time.deltaTime);
			_Plate.transform.position = trajectory;
			_Player.transform.position = trajectory += new Vector3 (0, 0.5f, 0);

			if (_Plate.transform.position == _CurrentDestination) {
				_Index++;

				if (_Index >= _Targets.Count) {
					_Moving = false;
					EnableDefaultControllingSystem (true);
				} else {
					_CurrentDestination = _Targets [_Index].position;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		if (!_Transformed) {
			if (other.CompareTag ("Player")) {
				_Player = other.gameObject;
				_Moving = true;
				_Transformed = true;
				other.transform.position = _Targets [0].position + new Vector3 (0, 0.5f, 0);
				EnableDefaultControllingSystem (false);
			}
		}
	}

	// Enable default controlling scripts and components
	public void EnableDefaultControllingSystem(bool enable)
	{
		_VController.lockMovement = !enable;
		_VController.isJumping = false;
		_VController.isWalking = false;
		_VController.enabled = enable;

		/*
		_Rigidbody.isKinematic = !enable;
		if (!enable)
		{
			_VController.isJumping = false;
		}
		*/
	}

}
