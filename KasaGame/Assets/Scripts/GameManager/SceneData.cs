using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData {
	public GameObject[] checkPoints;
	public GameObject currentCheckpoint;
	public int health;
	public GameObject[] items;
}

class CheckPointData {
	public int pos;
	public bool isActivated;
}

class ItemData {
	public string id;
	public string partnerId;
}