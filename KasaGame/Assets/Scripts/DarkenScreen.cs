using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkenScreen : MonoBehaviour {
	[SerializeField] private float darkenTime = 1f;
	private float alpha = 1f;
	private bool fadingOut = false;
	private bool fadingIn = false;
	[SerializeField] private Texture2D tex;
	private Pixelate pixelate;

	private float t = 0.0f;
	
	void Start()
	{
		pixelate = GetComponent<Pixelate>();	
	}
	void Update () 
	{
		DeathFade();
	}

	public void FadeOut()
	{
		fadingOut = true;
	}

	public void FadeIn() 
	{
		fadingIn = true;
	}

	public void DeathFade()
	{
		if (fadingOut)
		{
			Debug.Log("pixel");
			pixelate.pixelSizeX = Mathf.RoundToInt(Mathf.Lerp(1f, 20f, t));
			pixelate.pixelSizeY = Mathf.RoundToInt(Mathf.Lerp(1f, 20f, t));
			
        	t += darkenTime * Time.deltaTime;
			if (pixelate.pixelSizeX == 20f)
			{
				t = 0f;
				fadingOut = false;
				fadingIn = true;
				GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("Alive", true);
			}
		}
		else if (fadingIn)
		{
			Debug.Log("pixel");
			pixelate.pixelSizeX = (int) Mathf.Lerp(20f, 1f, t);
			pixelate.pixelSizeY = (int) Mathf.Lerp(20f, 1f, t);
			t += darkenTime * Time.deltaTime;
			if (pixelate.pixelSizeX == 1f)
			{
				t = 0f;
				fadingIn = false;
			}
		}
	}

	void OnGUI()
	{
		if (fadingIn || fadingOut)
		{
			if (fadingOut)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, t);
				GUI.depth = -1000;
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
			}
			else if (fadingIn)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - t);
				GUI.depth = -1000;
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
			}
		}
	}
}
