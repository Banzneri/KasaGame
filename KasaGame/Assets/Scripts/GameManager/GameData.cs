using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
	public int health = 3;
	public string currentSceneName = "Level_Hub 1";
	public int currentSceneId = 0;
	public MyVector3 currentPosition = new MyVector3(0, 0, 0);
	public MyQuaternion currentRotation = new MyQuaternion(0, 0, 0, 0);
	public bool hasPlayed = false;
	public int levelsUnlocked = 1;
	public bool redMacGuffin = false;
	public bool greenMacGuffin = true;
	public bool blueMacGuffin = false;
	public bool level1done = false;
	public bool level2done = false;
	public bool level3done = false;
	public bool level4done = false;
	public bool level5done = false;
	public bool level6done = false;
	public bool level7done = false;
	public bool level8done = false;
	public bool level9done = false;
	public bool level10done = false;
}
