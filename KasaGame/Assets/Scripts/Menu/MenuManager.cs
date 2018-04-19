using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	[SerializeField] private GameObject menuCanvasObj;
	[SerializeField] private Button resumeButton;
	[SerializeField] private Button quitButton;
	[SerializeField] private Button hubButton;

	public static bool isPaused = false;
	
	// Update is called once per frame
	void Update () {
		HandleMenu();
	}

	public void LaunchGameMenu()
	{
		menuCanvasObj.SetActive(true);
		isPaused = true;
		Time.timeScale = 0f;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void CloseGameMenu()
	{
		menuCanvasObj.SetActive(false);
		isPaused = false;
		Time.timeScale = 1f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ReturnToHub()
	{
		Time.timeScale = 1f;
		isPaused = false;
		MySceneManager.LoadLevel("Level_Hub 1");
	}

	private void HandleMenu()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!isPaused)
			{
				LaunchGameMenu();
			} 
			else
			{
				CloseGameMenu();
			}
		}
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1f;
		isPaused = false;
		MySceneManager.LoadMainMenu();
	}
}
