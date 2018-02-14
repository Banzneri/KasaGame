using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class MyCharManager : MonoBehaviour {
	[SerializeField] private float _currentHealth = 3f;
	[SerializeField] private float _maxHealth = 3f;
	[SerializeField] private float _damageCooldown = 2f;
	[SerializeField] private float _immunityBlinkRate = 0.3f;
	[SerializeField] private float _jumpFrames = 0.05f;
	[SerializeField] private float _extraFallSpeed = 0.8f;
	[SerializeField] private float _maxStamina = 100f;
	[SerializeField] private float _currentStamina = 100f;
	[SerializeField] private bool _blinks = true;
	private GameObject[] checkpoints;
	private float _damageCooldownCounter = 0f;
	private float _immunityBlinkCounter = 0f;
	private bool _immuneToDamage = false;
	private bool _pressingJump = false;
	private float _jumpCounter = 0f;
	private Rigidbody _rigidbody;
	private Color[] _originalColors;
	private float _yBeforeJump = 0f;

	private bool CanMove = true;
	private SkinnedMeshRenderer _renderer;
	private vThirdPersonController cc;
	// Use this for initiapyusization

	public float Health { get { return _currentHealth; } 
							set {_currentHealth = value; } }
	public float CurrentStamina { get { return _currentStamina; } 
							set {_currentStamina = value; } }
	public float MaxStamina { get { return _maxStamina; } }

	void Start () {
		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		if (_blinks) InitBlinking();
		_rigidbody = GetComponent<Rigidbody>();
		cc = GetComponent<vThirdPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
		HandleDamageCooldown();
		HandleFallSpeed();
		HandleJumpFrames();
	}

	private void InitBlinking()
	{
		_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		Color color1 = _renderer.materials[0].color;
		Color color2 = _renderer.materials[1].color;
		_originalColors = new Color[] { new Color(color1.r, color1.g, color1.b), new Color(color2.r, color2.g, color2.b) };
	}

	private void HandleDamageCooldown() 
	{
		if (_immuneToDamage)
		{
			
			Debug.Log("ImmuneToDamage");
			_damageCooldownCounter += Time.deltaTime;
			_immunityBlinkCounter += Time.deltaTime;

			if (_damageCooldownCounter > _damageCooldown)
			{
				_immuneToDamage = false;
				_damageCooldownCounter = 0f;
				if (_blinks) ReturnOriginalColor();
			}
			else if (_blinks) HandleBlinking();
		}
	}

	private void HandleBlinking()
	{
		Color materialColor = _renderer.materials[0].color;
		if (_immunityBlinkCounter > _immunityBlinkRate)
		{
			if (materialColor == Color.red) ReturnOriginalColor();
			else ChangeToBlinkColor();
			_immunityBlinkCounter = 0f;
		}
		if (!_immuneToDamage && _renderer.materials[0].color == Color.red)
		{
			ReturnOriginalColor();
		}
	}

	private bool IsWalking() 
	{

		return false;
	}

	private void ReturnOriginalColor() {
		_renderer.materials[0].color = _originalColors[0];
		_renderer.materials[1].color = _originalColors[1];
	}

	private void ChangeToBlinkColor() 
	{
		_renderer.materials[0].color = Color.red;
		_renderer.materials[1].color = Color.red;
	}

	private void HandleFallSpeed()
	{
		if (_rigidbody.velocity.y < 3f)
		{
			Vector3 vel = _rigidbody.velocity;
            vel.y -= _extraFallSpeed;
            _rigidbody.velocity = vel;
		}
	}

	public void ReturnToClosestCheckpoint() {
		Camera.main.GetComponent<DarkenScreen>().FadeOut();
		List<GameObject> activeCheckpoints = new List<GameObject>();

		for (int i = 0; i < checkpoints.Length; i++)
		{
			if (checkpoints[i].GetComponent<RotateGear>().isActivated)
			{
				activeCheckpoints.Add(checkpoints[i]);
			}
		}

		GameObject closestActiveCheckPoint = activeCheckpoints[0];

		foreach (GameObject point in activeCheckpoints)
		{
			if (closestActiveCheckPoint == point) continue;

			Vector3 curClosestCheckPoint = closestActiveCheckPoint.transform.position;
			Vector3 maybeClosestCheckPoint = point.transform.position;
			float curDistance = Vector3.Distance(transform.position, curClosestCheckPoint);
			float newDistance = Vector3.Distance(transform.position, maybeClosestCheckPoint);
			
			if (newDistance < curDistance) closestActiveCheckPoint = point;
		}

		transform.position = closestActiveCheckPoint.GetComponent<RotateGear>().spawnPoint.position;
	}

	public void TakeDamage() {
		if (!_immuneToDamage)
		{
			_currentHealth--;
			_immuneToDamage = true;
		}
		if (_currentHealth == 0)
		{	
			_damageCooldownCounter = 0f;
			_immunityBlinkCounter = 0f;
			_immuneToDamage = false;
			_currentHealth = _maxHealth;
			ReturnToClosestCheckpoint();
		}
	}

	public void HandleJumpFrames() 
	{
		if (Input.GetButtonDown("Jump") && cc.isGrounded)
		{
			_yBeforeJump = _rigidbody.position.y;
			_pressingJump = true;
		}

		if (Input.GetButtonUp("Jump"))
		{
			_pressingJump = false;
			_jumpCounter = 0f;
		}

		if (_pressingJump)
		{
			_jumpCounter += Time.deltaTime;
			if (_jumpCounter < _jumpFrames)
			{
				Vector3 vel = _rigidbody.velocity;
				vel.y += 1f;
				_rigidbody.velocity = vel;
			}
			else
			{
				_pressingJump = false;
				_jumpCounter = 0f;
				Vector3 vel = _rigidbody.velocity;
				vel.y = 8.5f;
				_rigidbody.velocity = vel;
				Vector3 pos = _rigidbody.position;
				pos.y = _yBeforeJump + 1f;
				_rigidbody.position = pos;
			}
			Debug.Log(_rigidbody.position.y);
		}
	}
}
