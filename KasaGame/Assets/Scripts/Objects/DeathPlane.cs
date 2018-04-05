using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour {
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<MyCharManager>().Die();
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<MyCharManager>().Die();
        }
    }
}
