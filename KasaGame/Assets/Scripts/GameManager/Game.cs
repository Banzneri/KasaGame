using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	private MyCharManager _player;
	private SceneHandler _currentScene;
	// Use this for initialization
	void Awake () {
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
		_currentScene = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveGame()
	{

	}

	public void LoadGame()
	{
		
	}
}
