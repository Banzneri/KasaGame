using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMainMenu : MonoBehaviour {

	void Start()
    {
        UnityEngine.UI.Button btn = GetComponent<UnityEngine.UI.Button>().GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Return);
    }
	void Return()
	{
		MySceneManager manager = Object.FindObjectOfType<MySceneManager>();
		manager.LoadMainMenu();
	}
}
