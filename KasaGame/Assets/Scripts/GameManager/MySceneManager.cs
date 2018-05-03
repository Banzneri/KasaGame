using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	private Scene currentScene;

	// Use this for initialization
	void Awake () {
		LoadMainMenu();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	static public void LoadLevel(string level)
	{
		SceneManager.LoadScene(level);
	}

	static public void LoadSettingsMenu()
	{
		SceneManager.LoadScene("SettingsMenu");
	}

	static public void RemoveSave(string sceneName)
	{
		File.Delete(Application.persistentDataPath + "/" + sceneName);
	}

	public void LaunchPauseMenu()
	{
		
	}
}
