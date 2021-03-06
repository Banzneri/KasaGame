﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData {
	public List<CheckPointData> checkPoints = new List<CheckPointData>();
	public CheckPointData currentCheckpoint;
	public List<MovableObjectData> movingPlatforms = new List<MovableObjectData>();
	public List<MovableObjectData> pushableBoxes = new List<MovableObjectData>();
	public List<SwitchData> switches = new List<SwitchData>();
	public List<DoorData> doors = new List<DoorData>();
	public List<ItemData> pickableItems = new List<ItemData>();
	public List<CheckPointData> screws = new List<CheckPointData>();
	public MyVector3 currentLocation;
	public MyQuaternion currentRotation;
}

[System.Serializable]
public class CheckPointData : SaveableObject {
	public bool isActivated;

	public CheckPointData(GameObject obj, bool active): base(obj) {
		isActivated = active;
	} 
}

[System.Serializable]
public class DoorData : SaveableObject {
	public bool isOpen;
	public bool key;

	public DoorData(GameObject obj, bool open, bool hasKey): base(obj) {
		isOpen = open;
		key = hasKey;
	}
}

[System.Serializable]
public class ItemData: SaveableObject {
	public bool pickedUp;

	public ItemData(GameObject obj, bool isPickedUp) : base(obj) {
		pickedUp = isPickedUp;
	}
}

[System.Serializable]
public class SwitchData : SaveableObject {
	public bool activated;

	public SwitchData(GameObject obj, bool active): base(obj) {
		activated = active;
	}
}

[System.Serializable]
public class MovableObjectData : SaveableObject {
	public MyVector3 currentLocation;
	public bool goingToEndLoc;

	public MovableObjectData(GameObject obj, bool goingToEnd): base(obj) 
	{ 
		currentLocation = new MyVector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
		goingToEndLoc = goingToEnd;
	}
}

[System.Serializable]
public class SaveableObject {
	public MyVector3 pos;
	public MyQuaternion rot;

	public SaveableObject(GameObject obj) {
		pos = new MyVector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
		rot = new MyQuaternion(obj.transform.rotation.x, obj.transform.rotation.y, obj.transform.rotation.z, obj.transform.rotation.w);
	}
}

[System.Serializable]
public struct MyVector3 {
	public float x;
	public float y;
	public float z;

	public MyVector3(float rx, float ry, float rz)
	{
		x = rx;
		y = ry;
		z = rz;
	}
}

[System.Serializable]
public struct MyQuaternion {
	public float x;
	public float y;
	public float z;
	public float w;

	public MyQuaternion(float rx, float ry, float rz, float rw)
	{
		x = rx;
		y = ry;
		z = rz;
		w = rw;
	}
}