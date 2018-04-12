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
	[SerializeField] private GameObject _hat;
	[SerializeField] private float attackTime = 1f;
	[SerializeField] private float hitTime = 0.5f;
	[SerializeField] private AudioSource _gruntSound;
	[SerializeField] private AudioSource _deathSound;
	[SerializeField] private ClimbingBehaviour _climbingBehaviour;
	
	private float attackCounter = 0f;
	private float hitCounter = 0f;
	private float _jumpCounter = 0f;
	private float _deathCounter = 0f;
	private float _damageCooldownCounter = 0f;
	private float _immunityBlinkCounter = 0f;

	private bool wantsToHit = false;
	private GameObject[] checkpoints;
	private bool _immuneToDamage = false;
	private bool _pressingJump = false;
	private bool _jumping = false;
	private Rigidbody _rigidbody;
	private Color _originalColor;
	private float _yBeforeJump = 0f;
	private bool CanMove = true;
	private SkinnedMeshRenderer _renderer;
	private MeshRenderer _hatRenderer;
	private vThirdPersonController cc;
	private float _deathWait = 2f;
	private float _jumpForce = 0f;
	private float _maxJumpForce = 40f;
	private float _lastJumpTime = 0f;
	private float _lastJumpTimeCooldown = 0.2f;

	public bool throwing = false;
	public bool attackHitting = false;
	public bool attacking = false;
	public bool hitting = false;

	public float Health { get { return _currentHealth; } 
							set {_currentHealth = value; } }
	public float CurrentStamina { get { return _currentStamina; } 
							set {_currentStamina = value; } }
	public float MaxStamina { get { return _maxStamina; } }
	public bool Immune { get { return _immuneToDamage; } }

	public bool IsHitting { get { return hitCounter < hitTime; } }

	void Awake () {
		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		if (_blinks) InitBlinking();
		_rigidbody = GetComponent<Rigidbody>();
		cc = GetComponent<vThirdPersonController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		HandleDamageCooldown();
		HandleDying();
		HandleWeapon();
	}

	private void InitBlinking()
	{
		_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		_hatRenderer = _hat.GetComponent<MeshRenderer>();
		_originalColor = _renderer.materials[0].color;
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
			if (!_renderer.gameObject.activeSelf) ReturnOriginalColor();
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
		_renderer.gameObject.SetActive(true);
		_hatRenderer.gameObject.SetActive(true);
		_back.GetComponent<Renderer>().gameObject.SetActive(true);
	}

	private void ChangeToBlinkColor() 
	{
		_renderer.gameObject.SetActive(false);
		_hatRenderer.gameObject.SetActive(false);
		_back.GetComponent<MeshRenderer>().gameObject.SetActive(false);
	}

	public void ReturnToClosestCheckpoint() {
		RotateGear checkpoint = GetClosestCheckpoint().GetComponent<RotateGear>();
		transform.position = checkpoint.spawnPoint.position;
		transform.rotation = checkpoint.spawnPoint.rotation;
	}

	public GameObject GetClosestCheckpoint() {
		List<GameObject> activeCheckpoints = new List<GameObject>();

		// Populate the activeCheckpoints list
		for (int i = 0; i < checkpoints.Length; i++)
		{
			if (checkpoints[i].GetComponent<RotateGear>().isActivated)
			{
				activeCheckpoints.Add(checkpoints[i]);
			}
		}

		GameObject closestActiveCheckPoint = activeCheckpoints[0];

		// Determine the closest active checkpoint
		foreach (GameObject point in activeCheckpoints)
		{
			if (closestActiveCheckPoint == point) continue;

			Vector3 curClosestCheckPoint = closestActiveCheckPoint.transform.position;
			Vector3 maybeClosestCheckPoint = point.transform.position;
			float curDistance = Vector3.Distance(transform.position, curClosestCheckPoint);
			float newDistance = Vector3.Distance(transform.position, maybeClosestCheckPoint);
			
			if (newDistance < curDistance) closestActiveCheckPoint = point;
		}

		return closestActiveCheckPoint;
	}

	public void TakeDamage() {
		if (!_immuneToDamage)
		{
			_climbingBehaviour.ChangeMode(new ModeOnAir(_climbingBehaviour, false));
			if (_currentHealth > 1)
			{
				GetComponent<Animator>().SetTrigger("Stun");
				_gruntSound.time = 0.5f;
				_gruntSound.Play();
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
			if (hitting) hitCounter += Time.deltaTime;
			attackCounter += Time.deltaTime;
			if (hitCounter > hitTime)
			{
				hitting = false;
				hitCounter = 0.0f;
			}
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
		else if ( _hand.activeSelf)
		{
			WeaponInBack();
		}

		if (GetComponent<Animator>().GetBool("pushing"))
		{
			attacking = false;
			hitting = false;
			WeaponInBack();	
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
		if (hitting)
		{
			wantsToHit = true;
		}
		if (!throwing  && !hitting && !GetComponent<Animator>().GetBool("pushing"))
		{
			attacking = true;
			hitting = true;
			attackCounter = 0f;
			hitCounter = 0f;
			wantsToHit = false;
			_hand.GetComponent<AudioSource>().Play();
			GetComponent<vThirdPersonAnimator>().Attack();
		}
	}

	public void ThrowWeapon()
	{
		if (!throwing)
		{
			Instantiate(_weapon, _hand.transform.position, Quaternion.Euler(Vector3.zero));
			WeaponFlying();
			attacking = false;
			hitting = false;
			throwing = true;
		}
	}

	public bool CanJump() {
		// float velY = _rigidbody.velocity.y;
		return cc.isGrounded;
	}

	public void Die()
	{
		PlayDeathSound();
		GetComponent<Animator>().SetBool("Alive", false);
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
		PlayRespawnSound();
		cc.isMovable = true;
		_deathCounter = 0f;
		_currentHealth = _maxHealth;
		ReturnToClosestCheckpoint();
		Camera.main.GetComponent<vThirdPersonCamera>().SetCameraBehindPlayer();
	}

	private void PlayDeathSound()
	{
		_deathSound.time = 0.0f;
		_deathSound.pitch = 1.0f;
		_deathSound.Play();
	}

	private void PlayRespawnSound()
	{
		_deathSound.time = 2.4f;
		_deathSound.pitch = -1.2f;
		_deathSound.Play();
	}
}
