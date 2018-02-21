using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour {
	private MyCharManager player;
	private bool hasActivated = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		hasActivated = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Interactable" && !hasActivated && player.IsHitting)
		{
			other.gameObject.GetComponent<ITriggerObject<IActionObject>>().TriggerAll();
			hasActivated = true;
		}

		if (!player.IsHitting)
		{
			hasActivated = false;
		}
	}
}
