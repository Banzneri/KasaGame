using System.Collections;
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

	private Vector3 defaultStartPosition;

	private MyCharManager player;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
		Init();
		LoadScene();
		Debug.Log(player.Health);
	}

	private void Init() {
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
			MovableObjectData data = new MovableObjectData(box);
			sceneData.pushableBoxes.Add(data);
		}

		foreach (var platform in movingPlatforms)
		{
			MovableObjectData data = new MovableObjectData(platform);
			sceneData.movingPlatforms.Add(data);
		}

		foreach (var key in keys)
		{
			ItemData data = new ItemData(key, !key.activeSelf);
			sceneData.pickableItems.Add(data);
		}

		foreach (var door in doors)
		{
			DoorData data = new DoorData(door, door.GetComponent<Door>().open);
			sceneData.doors.Add(data);
		}

		foreach (var screw in screws)
		{
			CheckPointData data = new CheckPointData(screw, screw.GetComponent<Screw>().down);
			sceneData.screws.Add(data);
		}

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
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/PlayerData");
		GameData gameData = new GameData();

		gameData.health = (int)player.Health;
		gameData.currentSceneName = SceneManager.GetActiveScene().name;
		Vector3 pos = player.GetClosestCheckpoint().transform.position;
		gameData.currentPosition = new MyVector3(pos.x, pos.y, pos.z);
		bf.Serialize(fs, gameData);
		fs.Close();
	}

	public void LoadScene()
	{
		SceneData data = null;

		try
        {
            using (FileStream file = File.Open(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name, FileMode.Open))
            {
				BinaryFormatter bf = new BinaryFormatter();
				data = (SceneData)bf.Deserialize(file);
				file.Close();
				LoadPlayer();
            }
        }
        catch (FileNotFoundException ex)
        {
			Debug.Log("Save not found, creating new!");
			data = CreateSceneDataObject();
			LoadPlayer();
			player.gameObject.transform.position = defaultStartPosition;
        }

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
			movingPlatforms[i].transform.position = TranslateMyVector3ToVector3(data.movingPlatforms[i].pos);
			movingPlatforms[i].transform.rotation = TranslateMyQuaternionToQuaternion(data.movingPlatforms[i].rot);
		}

		for (int i = 0; i < data.pickableItems.Count; i++)
		{
			keys[i].transform.position = TranslateMyVector3ToVector3(data.pickableItems[i].pos);
			keys[i].transform.rotation = TranslateMyQuaternionToQuaternion(data.pickableItems[i].rot);
			keys[i].SetActive(!data.pickableItems[i].pickedUp);
		}

		for (int i = 0; i < data.doors.Count; i++)
		{
			doors[i].GetComponent<Door>().open = data.doors[i].isOpen;
		}

		for (int i = 0; i < data.screws.Count; i++)
		{
			screws[i].GetComponent<Screw>().down = data.screws[i].isActivated;
			screws[i].GetComponent<Screw>().loaded = true;
		}
		//ClearAll();

		Debug.Log(checkpoints.Count);
	}

	public void LoadPlayer()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/PlayerData", FileMode.Open);
		GameData data = (GameData)bf.Deserialize(file);
		player.Health = data.health;
		player.transform.position = TranslateMyVector3ToVector3(data.currentPosition);
		file.Close();
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
