using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayerOnCollision : MonoBehaviour {
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
