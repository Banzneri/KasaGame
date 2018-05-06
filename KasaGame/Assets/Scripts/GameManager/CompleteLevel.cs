using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!HasFinishedLevel(SceneManager.GetActiveScene().name))
			{
				Debug.Log("NotHasFinished");
				Complete(SceneManager.GetActiveScene().name);
			}
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SavePlayer();
			MySceneManager.LoadLevel("Level_Hub 1");
		}
	}

	static bool HasFinishedLevel(string levelName)
	{
		GameData data = Game.GetGameData();

		switch (levelName)
		{
			case "LavaRuins1":
				return data.level1done;
			case "TropicEasy":
				return data.level2done;
			case "CaveTemple2":
				return data.level3done;
			case "LavaRuins2":
				return data.level4done;
			case "TropicMedium":
				return data.level5done;
			case "CaveTemple1":
				return data.level6done;
			case "LavaRuins3":
				return data.level7done;
			case "TropicHard":
				return data.level8done;
			case "CaveTemple3":
				return data.level9done;
		}

		return false;
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
