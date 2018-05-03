using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class ThrowWeapon : MonoBehaviour {
	[SerializeField] private float throwDistance = 5f;
	[SerializeField] private float throwSpeed = 50f;
	[SerializeField] private float rotateSpeed = 50f;

	private Vector3 destination;
	
	private vThirdPersonController player;
	private Transform hand;
	private bool comingBack = false;
	private bool hasActivated = false;
	private AudioSource _throwSound;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonController>();
		hand = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>()._hand.transform;
		destination = player.transform.forward;
		destination.z = Camera.main.transform.forward.z;
		destination.x = Camera.main.transform.forward.x;
		player.transform.forward = destination;
		hasActivated = false;
		_throwSound = gameObject.GetComponent<AudioSource>();
		_throwSound.Play();
		Vector3 pos = transform.position;
		pos.y += 0.3f;
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
		if (Vector3.Distance(transform.position, player.transform.position) > throwDistance && !comingBack)
		{
			comingBack = true;
		}

		if (!comingBack)
		{	
			transform.position += destination * Time.deltaTime * throwSpeed;
		}
		else
		{
			Vector3 pos = Vector3.MoveTowards(transform.position, hand.position, Time.deltaTime * throwSpeed);
			transform.position = pos;
		}

		if (transform.position == hand.position && comingBack)
		{	
			player.GetComponent<MyCharManager>().throwing = false;
			player.GetComponent<MyCharManager>().WeaponInHand();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
			other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			comingBack = true;
		}
		if ( (other.tag == "Screw" || other.tag =="Button") && !hasActivated)
		{
			other.gameObject.GetComponent<ITriggerObject<IActionObject>>().TriggerAll();
			hasActivated = true;
		}
		if (other.gameObject.tag == "Rock")
		{
			other.gameObject.GetComponent<Rock>().DestroyRock();
		}
	}

	private void OnCollisionEnter(Collision other) {
		
	}
}
