using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour {
	[SerializeField] private float speed;
	[SerializeField] private float maxAngle;
	[SerializeField] private float direction = 1.0f;
	private float current = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		current += Time.fixedDeltaTime * direction * speed;
		if (current > maxAngle)
		{
			current = maxAngle;
			direction = -direction;
		}
		else if (current < -maxAngle)
		{
			current = -maxAngle;
			direction = -direction;
		}
		Vector3 rotation = new Vector3(current, 0, 0);
		transform.rotation = Quaternion.Euler(rotation);
	}
}
