using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private Animator animator;

	private Transform playerTransform;
	[SerializeField] private GameObject wheel1;
	[SerializeField] private GameObject wheel2;

	[SerializeField] private float speed;
	[SerializeField] private float attackDistance = 5f;
	private bool moving = false;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		animator.SetBool("Moving", true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsCloseToPlayer())
		{
			if (!moving)
			{
				moving = true;
				SetWheelVelocity(50 * speed);	
			}
		}
		else
		{
			if (moving)
			{
				moving = false;	
			}
			//animator.SetTrigger("Hit");
			SetWheelVelocity(0);	
		}
		if (moving)
		{
			ApproachPlayer();
		}
		animator.SetBool("Moving", moving);
		transform.LookAt(playerTransform);
	}

	void SetWheelVelocity(float vel)
	{
		wheel1.GetComponent<Rotate>().SetSpeed(vel);
		wheel2.GetComponent<Rotate>().SetSpeed(vel);
	}

	void ApproachPlayer()
	{
		transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
	}

	bool IsCloseToPlayer()
	{
		return Vector3.Distance(transform.position, playerTransform.position) < attackDistance;
	}
}
