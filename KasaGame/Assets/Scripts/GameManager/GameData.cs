using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
	public int health = 3;
	public string currentSceneName = "Level_Hub 1";
	public MyVector3 currentPosition = new MyVector3(0, 0, 0);
	public MyQuaternion currentRotation = new MyQuaternion(0, 0, 0, 0);
	public bool hasPlayed = false;
	public int levelsUnlocked = 1;
	public bool redMacGuffin = false;
	public bool greenMacGuffin = true;
	public bool blueMacGuffin = false;
}
