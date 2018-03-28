using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	private MyCharManager _player;
	public SceneHandler _currentScene;

	public GameData GetGameData()
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
}
