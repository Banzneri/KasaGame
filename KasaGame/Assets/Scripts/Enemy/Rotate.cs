using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	[SerializeField] private float rotateSpeed = 20;
	[SerializeField] private int rotateDirection = -1;
	[SerializeField] private Axis axis;

	enum Axis { X, Y, Z }
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (axis)
		{
			case Axis.X: 
				transform.Rotate(new Vector3(rotateSpeed * Time.deltaTime * rotateDirection, 0, 0));
				break;
			case Axis.Y: 
				transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime * rotateDirection, 0));
				break;
			case Axis.Z:
				transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime * rotateDirection));
				break; 
		}
	}

	public void SetSpeed(float speed) 
	{
		rotateSpeed = speed;
	}
}
