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
	[SerializeField] private float _jumpHeight = 5f;
	[SerializeField] public GameObject _back;
	[SerializeField] public GameObject _hand;
	[SerializeField] private GameObject _weapon;
	[SerializeField] private float attackTime = 1f;
	
	public bool throwing = false;
	private float attackCounter = 0f;

	public bool attacking = false;
	private GameObject[] checkpoints;
	private float _damageCooldownCounter = 0f;
	private float _immunityBlinkCounter = 0f;
	private bool _immuneToDamage = false;
	private bool _pressingJump = false;
	private bool _jumping = false;
	private float _jumpCounter = 0f;
	private Rigidbody _rigidbody;
	private Color[] _originalColors;
	private float _yBeforeJump = 0f;

	private bool _reachedApex = false;

	private bool CanMove = true;
	private SkinnedMeshRenderer _renderer;
	private vThirdPersonController cc;
	private float _deathWait = 2f;
	private float _deathCounter = 0f;

	private float _jumpForce = 0f;
	private float _maxJumpForce = 40f;

	public float Health { get { return _currentHealth; } 
							set {_currentHealth = value; } }
	public float CurrentStamina { get { return _currentStamina; } 
							set {_currentStamina = value; } }
	public float MaxStamina { get { return _maxStamina; } }
	public bool Immune { get { return _immuneToDamage; } }

	void Start () {
		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		if (_blinks) InitBlinking();
		_rigidbody = GetComponent<Rigidbody>();
		cc = GetComponent<vThirdPersonController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		HandleDamageCooldown();
		AltHandleJumpFrames();
		HandleFallSpeed();
		HandleDying();
		HandleWeapon();
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
		if (_jumping)
		{
			Vector3 vel = _rigidbody.velocity;
			
			if (_rigidbody.velocity.y < 0 && !cc.isGrounded)
			{
				vel.y -= _extraFallSpeed;
			}
			if (_rigidbody.velocity.y < 4f && !cc.isGrounded)
			{
				if (!_reachedApex)
				{	
					//vel.y -= 1f;
					_reachedApex = true;	
				}
				vel.y -= _extraFallSpeed / 2f;
			}
			_rigidbody.velocity = vel;
		} 
		else if (!cc.isGrounded)
		{
			Vector3 vel = _rigidbody.velocity;
			vel.y -= 0.4f;
			_rigidbody.velocity = vel;
		}
	}

	public void ReturnToClosestCheckpoint() {
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
			if (_currentHealth > 1)
			{
				GetComponent<Animator>().SetTrigger("Stun");
			}
			if (_currentHealth > 0f)
			{
				_currentHealth--;
			}
			_immuneToDamage = true;
		}
		if (_currentHealth == 0f && _deathCounter == 0f)
		{	
			Die();
			GetComponent<Animator>().SetTrigger("Die");
			GetComponent<Animator>().SetBool("Alive", false);
		}
	}

	public void HandleDying()
	{
		if (_currentHealth == 0)
		{
			_deathCounter += Time.deltaTime;
			if (_deathCounter > _deathWait)
			{
				Respawn();
			}
		}
	}

	public void HandleWeapon()
	{
		if (attacking)
		{
			attackCounter += Time.deltaTime;
			if (attackCounter > attackTime)
			{
				attacking = false;
				attackCounter = 0f;
			}

			if (_back.activeSelf)
			{
				WeaponInHand();
			}
		}
		else if ( !attacking && _hand.activeSelf )
		{
			WeaponInBack();
		}
		else if (throwing && _hand.activeSelf)
		{
			
		}
	}

	public void WeaponInBack()
	{
		_hand.SetActive(false);
		_back.SetActive(true);
	}

	public void WeaponInHand()
	{
		_hand.SetActive(true);
		_back.SetActive(false);
	}

	public void WeaponFlying()
	{
		_hand.SetActive(false);
		_back.SetActive(false);
	}

	public void Attack()
	{
		if (!throwing)
		{
			attacking = true;
			attackCounter = 0f;	
		}
	}

	public void ThrowWeapon()
	{
		if (!throwing)
		{
			Instantiate(_weapon, _hand.transform.position, Quaternion.Euler(Vector3.zero));
			WeaponFlying();
			throwing = true;
		}
	}

	public void Die()
	{
		_currentHealth = 0f;
		cc.isMovable = false;
		cc.speed = 0f;
		Camera.main.GetComponent<DarkenScreen>().FadeOut();
		_damageCooldownCounter = 0f;
		_immunityBlinkCounter = 0f;
		_immuneToDamage = false;
	}

	private void Respawn()
	{
		cc.isMovable = true;
		_deathCounter = 0f;
		_currentHealth = _maxHealth;
		ReturnToClosestCheckpoint();
	}

	public void AltHandleJumpFrames()
	{
		Vector3 vel = _rigidbody.velocity;
		if (cc.isGrounded && _jumping && vel.y == 0)
		{
			_reachedApex = false;
			_jumping = false;
		}
		if (Input.GetButtonDown("Jump") && cc.isGrounded)
		{
			_yBeforeJump = _rigidbody.position.y;
			_pressingJump = true;
			_jumping = true;
		}

		if (Input.GetButtonUp("Jump"))
		{
			_pressingJump = false;
			_jumpCounter = 0f;
		}

		if (_pressingJump)
		{
			_jumpCounter += Time.deltaTime;
			if (_jumpCounter < cc.jumpTimer)
			{
				_jumpForce = _jumpCounter;
			}
			else
			{
				_jumpForce = cc.jumpTimer;
				_jumpForce = 0f;
				_pressingJump = false;
				_jumpCounter = 0f;
			}
			vel.y +=  _jumpForce * 50f;
			_rigidbody.velocity = vel;
		}
	}
}
