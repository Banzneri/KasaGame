﻿using System.Collections;
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
		if ( (other.gameObject.tag == "Screw" || other.gameObject.tag =="Button") && !hasActivated && player.hitting )
		{
			other.gameObject.GetComponent<ITriggerObject<IActionObject>>().TriggerAll();
			hasActivated = true;
		}

		if (!player.hitting)
		{
			hasActivated = false;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if ( (other.gameObject.tag == "Screw" || other.gameObject.tag =="Button") && !hasActivated && player.hitting )
		{
			other.gameObject.GetComponent<ITriggerObject<IActionObject>>().TriggerAll();
			hasActivated = true;
		}

		if (!player.hitting)
		{
			hasActivated = false;
		}
	}
}
