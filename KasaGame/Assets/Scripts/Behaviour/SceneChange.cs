using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour {
	[SerializeField] private string level;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			MySceneManager.LoadLevel(level);
		}
	}
}
