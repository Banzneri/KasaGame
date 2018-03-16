using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour {
	[SerializeField] private int level = 1;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Object.FindObjectOfType<MySceneManager>().LoadLevel("Level" + level);
		}
	}
}
