using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	private MyCharManager _player;
	public SceneHandler _currentScene;

	static public GameData GetGameData()
	{
		BinaryFormatter bf = new BinaryFormatter();
		try
        {
            using (FileStream file = File.Open(Application.persistentDataPath + "/PlayerData", FileMode.Open))
            {
				GameData gameData = (GameData) bf.Deserialize(file);
				file.Close();
				return gameData;
            }
        }
        catch (System.Exception ex)
        {
			FileStream file = File.Create(Application.persistentDataPath + "/PlayerData");
			GameData gameData = new GameData();
			bf.Serialize(file, gameData);
			file.Close();
			return gameData;
        }
		//ClearAll();
	}

	static public void SetGameData(GameData data)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/PlayerData");
		bf.Serialize(file, data);
		file.Close();
	}

	public static bool HasFinishedLevel(string levelName)
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

	public static bool HasFinishedLevel(int levelId)
	{
		GameData data = Game.GetGameData();

		switch (levelId)
		{
			case 1:
				return data.level1done;
			case 2:
				return data.level2done;
			case 3:
				return data.level3done;
			case 4:
				return data.level4done;
			case 5:
				return data.level5done;
			case 6:
				return data.level6done;
			case 7:
				return data.level7done;
			case 8:
				return data.level8done;
			case 9:
				return data.level9done;
			case 10:
				return data.level10done;
		}

		return false;
	}
}
