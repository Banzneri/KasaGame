﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] private Transform startLocation;
	[SerializeField] private Transform endLocation;
	[SerializeField] private float speed;
	[SerializeField] private float waitTime;

	private Vector3 currentDestination;
	private float waitCounter = 0;

	// Use this for initialization
	void Start () {
		currentDestination = new Vector3(startLocation.position.x, startLocation.position.y, startLocation.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 trajectory = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);
		transform.position = trajectory;

		if (transform.position == startLocation.position)
		{
			if (waitCounter == 0)
			{
				GetComponent<AudioSource>().Stop();
			}
			waitCounter += Time.deltaTime;
			if (waitCounter > waitTime)
			{
				currentDestination = endLocation.position;
				waitCounter = 0;
				GetComponent<AudioSource>().Play();
			}
		} 
		else if (transform.position == endLocation.position)
		{
			if (waitCounter == 0)
			{
				GetComponent<AudioSource>().Stop();
			}
			waitCounter += Time.deltaTime;
			if (waitCounter > waitTime)
			{
				currentDestination = startLocation.position;
				waitCounter = 0;
				GetComponent<AudioSource>().Play();
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.transform.parent = transform;
		}
	}

	void OnCollisionExit(Collision other)
	{
			if (other.gameObject.tag == "Player")
		{
			other.gameObject.transform.parent = null;
		}		
	}
}