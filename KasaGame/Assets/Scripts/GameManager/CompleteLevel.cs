using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!Game.HasFinishedLevel(SceneManager.GetActiveScene().name))
			{
				Debug.Log("NotHasFinished");
				Complete(SceneManager.GetActiveScene().name);
			}
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SavePlayer();
			MySceneManager.LoadLevel("Level_Hub 1");
		}
	}

	private void Complete(string levelName)
	{
		GameData data = Game.GetGameData();

		switch (levelName)
		{
			case "LavaRuins1":
				data.level1done = true;
				break;
			case "TropicEasy":
				data.level2done = true;
				break;
			case "CaveTemple2":
				data.level3done = true;
				break;
			case "LavaRuins2":
				data.level4done = true;
				break;
			case "TropicMedium":
				data.level5done = true;
				break;
			case "CaveTemple1":
				data.level6done = true;
				break;
			case "LavaRuins3":
				data.level7done = true;
				break;
			case "TropicHard":
				data.level8done = true;
				break;
			case "CaveTemple3":
				data.level9done = true;
				break;
			default:
				Debug.Log("Not funbdsda");
				break;
		}
		Debug.Log(levelName);
		data.levelsUnlocked += 1;
		Game.SetGameData(data);
	}
}
