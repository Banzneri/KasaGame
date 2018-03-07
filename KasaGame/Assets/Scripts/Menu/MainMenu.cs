using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Button startButton;
	[SerializeField] UnityEngine.UI.Button settingsButton;
	[SerializeField] UnityEngine.UI.Button quitButton;
	[SerializeField] UnityEngine.UI.Button backButton;
	[SerializeField] Canvas mainMenuCanvas;
	[SerializeField] Canvas settingsCanvas;
	MySceneManager sceneManager;

	void Awake () 
	{
		ActivateMainMenu();
		sceneManager = Object.FindObjectOfType<MySceneManager>();
		UnityEngine.UI.Button start = startButton.GetComponent<UnityEngine.UI.Button>();
		UnityEngine.UI.Button settings = settingsButton.GetComponent<UnityEngine.UI.Button>();
		UnityEngine.UI.Button quit = quitButton.GetComponent<UnityEngine.UI.Button>();
		UnityEngine.UI.Button back = backButton.GetComponent<UnityEngine.UI.Button>();

		start.onClick.AddListener(StartGame);
		settings.onClick.AddListener(ActivateSettings);
		quit.onClick.AddListener(QuitGame);
		back.onClick.AddListener(ActivateMainMenu);
	}

	private void StartGame()
	{
		sceneManager.LoadLevel(1);
	}

	private void ActivateSettings()
	{
		settingsCanvas.gameObject.SetActive(true);
		mainMenuCanvas.gameObject.SetActive(false);
	}

	private void ActivateMainMenu()
	{
		settingsCanvas.gameObject.SetActive(false);
		mainMenuCanvas.gameObject.SetActive(true);
	}

	private void QuitGame()
	{
		Application.Quit();
	}
}
