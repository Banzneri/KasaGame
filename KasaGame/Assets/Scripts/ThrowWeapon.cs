using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class ThrowWeapon : MonoBehaviour {
	[SerializeField] private float throwDistance = 5f;
	[SerializeField] private float throwSpeed = 50f;

	private Vector3 destination;
	
	private vThirdPersonController player;
	private Transform hand;
	private bool comingBack = false;
	private bool hasActivated = false;
	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<vThirdPersonController>();
		hand = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>()._hand.transform;
		destination = player.transform.forward;
		hasActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 25, 0));
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
		if (other.gameObject.tag != "Player")
		{
			comingBack = true;
		}
		if (other.gameObject.tag == "Interactable" && !hasActivated)
		{
			other.gameObject.GetComponent<ITriggerObject<IActionObject>>().TriggerAll();
			hasActivated = true;
		}
	}
}
