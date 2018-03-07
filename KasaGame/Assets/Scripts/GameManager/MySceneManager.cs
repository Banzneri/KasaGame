using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void LoadLevel(int level)
	{
		SceneManager.LoadScene("Level" + level);
	}

	public void LoadSettingsMenu()
	{
		SceneManager.LoadScene("SettingsMenu");
	}


}
