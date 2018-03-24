using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
	public int health = 3;
	public string currentSceneName = "Level2";
	public MyVector3 currentPosition = new MyVector3(0, 0, 0);
	public MyQuaternion currentRotation = new MyQuaternion(0, 0, 0, 0);
	public bool hasPlayed = false;
}
