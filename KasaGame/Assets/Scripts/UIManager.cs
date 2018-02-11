using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private RawImage[] hearts; 
	private MyCharManager player;

	// Use this for initialization
	void Start () {
		hearts = GameObject.FindObjectsOfType<RawImage>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.Health == 2)
		{
			if (hearts[0].enabled)
			{
				hearts[0].enabled = false;
			}
		}
		else if (player.Health == 1)
		{
			if (hearts[1].enabled)
			{
				hearts[1].enabled = false;
			}
		}
		
		if (player.Health == 3)
		{
			RefreshHearts();
		}
	}

	void RefreshHearts() 
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			if (!hearts[i].enabled)
			{
				hearts[i].enabled = true;
			}		
		}
	}
}
