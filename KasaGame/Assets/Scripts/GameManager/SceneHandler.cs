﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
	private List<GameObject> checkpoints = new List<GameObject>();
	private List<GameObject> movableBoxes = new List<GameObject>();
	private List<GameObject> movingPlatforms = new List<GameObject>();
	private List<GameObject> keys = new List<GameObject>();
	private List<GameObject> doors = new List<GameObject>();
	private List<GameObject> screws = new List<GameObject>();
	private UIManager ui;

	private Vector3 defaultStartPosition;

	private MyCharManager player;

	// Use this for initialization
	void Awake () {
		Init();
		LoadScene();
		LoadPlayer();
	}

	private void Init() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
		ui = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();

		checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
		movableBoxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
		movingPlatforms.AddRange(GameObject.FindGameObjectsWithTag("MovingPlatform"));
		keys.AddRange(GameObject.FindGameObjectsWithTag("Key"));
		doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
		screws.AddRange(GameObject.FindGameObjectsWithTag("Screw"));
		defaultStartPosition = GameObject.FindGameObjectWithTag("DefaultStartPosition").transform.position;
	}

	private SceneData CreateSceneDataObject()
	{
		SceneData sceneData = new SceneData();
		
		foreach (var checkpoint in checkpoints)
		{
			CheckPointData data = new CheckPointData(checkpoint, checkpoint.GetComponent<RotateGear>().isActivated);
			sceneData.checkPoints.Add(data);
		}

		foreach (var box in movableBoxes)
		{
			MovableObjectData data = new MovableObjectData(box, false);
			sceneData.pushableBoxes.Add(data);
		}

		foreach (var platform in movingPlatforms)
		{
			MovableObjectData data = new MovableObjectData(platform, platform.GetComponent<MovingPlatform>().goingToEndLoc);
			sceneData.movingPlatforms.Add(data);
		}

		foreach (var key in keys)
		{
			ItemData data = new ItemData(key, !key.activeSelf);
			sceneData.pickableItems.Add(data);
		}

		foreach (var door in doors)
		{
			DoorData data = new DoorData(door, door.GetComponent<Door>().open, door.GetComponent<Door>().doorKey);
			sceneData.doors.Add(data);
		}

		foreach (var screw in screws)
		{
			CheckPointData data = new CheckPointData(screw, screw.GetComponent<Screw>().down);
			sceneData.screws.Add(data);
		}
		Vector3 loc = player.GetClosestCheckpoint().GetComponent<RotateGear>().GetSpawnPoint().position;
		Quaternion rot = player.GetClosestCheckpoint().GetComponent<RotateGear>().GetSpawnPoint().rotation;

		sceneData.currentLocation = new MyVector3(loc.x, loc.y, loc.z);
		sceneData.currentRotation = new MyQuaternion(rot.x, rot.y, rot.z, rot.w);

		// GameObject curCheckpoint = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>().GetClosestCheckpoint();
		// sceneData.currentCheckpoint = new CheckPointData(curCheckpoint, curCheckpoint.GetComponent<RotateGear>().isActivated);
		return sceneData;
	}

	public void SaveScene()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name);

		SceneData data = CreateSceneDataObject();
		bf.Serialize(fs, data);
		fs.Close();
	}

	public void SavePlayer()
	{
		GameData gameData = Game.GetGameData();

		gameData.hasPlayed = true;
		gameData.health = (int)player.Health;
		gameData.currentSceneName = SceneManager.GetActiveScene().name;
		Game.SetGameData(gameData);
	}

	public void LoadScene()
	{
		SceneData data = GetSceneData();

		for (int i = 0; i < data.checkPoints.Count; i++)
		{
			checkpoints[i].GetComponent<RotateGear>().isActivated = data.checkPoints[i].isActivated;
			checkpoints[i].GetComponent<RotateGear>().rising = false;
		}

		for (int i = 0; i < data.pushableBoxes.Count; i++)
		{
			movableBoxes[i].transform.position = TranslateMyVector3ToVector3(data.pushableBoxes[i].pos);
			movableBoxes[i].transform.rotation = TranslateMyQuaternionToQuaternion(data.pushableBoxes[i].rot);
		}

		for (int i = 0; i < data.movingPlatforms.Count; i++)
		{
			movingPlatforms[i].GetComponent<MovingPlatform>().savedLocation = TranslateMyVector3ToVector3(data.movingPlatforms[i].currentLocation);
			movingPlatforms[i].transform.rotation = TranslateMyQuaternionToQuaternion(data.movingPlatforms[i].rot);
			movingPlatforms[i].GetComponent<MovingPlatform>().goingToEndLoc = data.movingPlatforms[i].goingToEndLoc;
		}

		for (int i = 0; i < data.pickableItems.Count; i++)
		{
			keys[i].transform.position = TranslateMyVector3ToVector3(data.pickableItems[i].pos);
			keys[i].transform.rotation = TranslateMyQuaternionToQuaternion(data.pickableItems[i].rot);
			keys[i].SetActive(!data.pickableItems[i].pickedUp);
		}

		bool hasKey = false;
		for (int i = 0; i < data.doors.Count; i++)
		{
			doors[i].GetComponent<Door>().open = data.doors[i].isOpen;
			doors[i].GetComponent<Door>().doorKey = data.doors[i].key;

			if (doors[i].GetComponent<Door>().doorKey && !hasKey)
			{
				hasKey = true;
			}
		}
		ui.hasKey = hasKey;

		for (int i = 0; i < data.screws.Count; i++)
		{
			screws[i].GetComponent<Screw>().down = data.screws[i].isActivated;
			screws[i].GetComponent<Screw>().loaded = true;
		}

		Debug.Log(checkpoints.Count);
	}

	public SceneData GetSceneData ()
	{
		SceneData data = null;
		bool firstTime = false;

		try
        {
            using (FileStream file = File.Open(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name, FileMode.Open))
            {
				BinaryFormatter bf = new BinaryFormatter();
				data = (SceneData)bf.Deserialize(file);
				file.Close();
            }
        }
        catch (System.Exception ex)
        {
			Debug.Log("Save not found, creating new!");
			data = CreateSceneDataObject();
			firstTime = true;
        }

		if (firstTime) SaveScene();

		return data;
	}

	public void LoadPlayer()
	{
		GameData data = Game.GetGameData();
		player.Health = data.health;
		player.transform.position = TranslateMyVector3ToVector3(GetSceneData().currentLocation);
		player.transform.rotation = TranslateMyQuaternionToQuaternion(GetSceneData().currentRotation);
	}

	public Vector3 TranslateMyVector3ToVector3(MyVector3 myVector3) {
		return new Vector3(myVector3.x, myVector3.y, myVector3.z);
	}

	public Quaternion TranslateMyQuaternionToQuaternion(MyQuaternion myQuaternion) {
		return new Quaternion(myQuaternion.x, myQuaternion.y, myQuaternion.z, myQuaternion.w);
	}

	public void ClearAll() {
		checkpoints.Clear();
		movableBoxes.Clear();
		keys.Clear();
		doors.Clear();
	}
}
