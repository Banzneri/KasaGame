using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRocks : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Rock")
		{
			other.GetComponent<Rock>().DestroyRock();
		}
	}
}
