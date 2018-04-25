using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour {
	[SerializeField] private string level;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SavePlayer();
			MySceneManager.LoadLevel(level);
		}
	}
}
