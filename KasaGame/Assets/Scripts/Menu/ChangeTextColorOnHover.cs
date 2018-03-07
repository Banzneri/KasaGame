using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ChangeTextColorOnHover : MonoBehaviour {
	// Use this for initialization
	Text text;
	void Start () {
		text = GetComponentInChildren<Text>();
	}

	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			
		}
	}
}
