using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	[SerializeField] Button startButton;
	[SerializeField] Button settingsButton;
	[SerializeField] Button quitButton;
	[SerializeField] Button backButton;
	[SerializeField] Canvas mainMenuCanvas;
	[SerializeField] Canvas settingsCanvas;
	MySceneManager sceneManager;

	void Awake () 
	{
		ActivateMainMenu();
		sceneManager = Object.FindObjectOfType<MySceneManager>();
		Button start = startButton.GetComponent<Button>();
		Button settings = settingsButton.GetComponent<Button>();
		Button quit = quitButton.GetComponent<Button>();
		Button back = backButton.GetComponent<Button>();

		start.onClick.AddListener(StartGame);
		settings.onClick.AddListener(ActivateSettings);
		quit.onClick.AddListener(QuitGame);
		back.onClick.AddListener(ActivateMainMenu);
		if (Object.FindObjectOfType<Game>().GetGameData().hasPlayed)
		{
			start.GetComponent<Text>().text = "Continue";
		}
	}

	private void StartGame()
	{
		sceneManager.LoadLevel(Object.FindObjectOfType<Game>().GetGameData().currentSceneName);
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
