using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour, ITriggerObject<IActionObject> {
	private Animator anim;
	public bool down = false;
	[HideInInspector]
	public bool loaded = false;
	public GameObject[] gameObjects;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (loaded)
		{
			if (down)
			{
				anim.SetTrigger("down");
				loaded = false;		
			}
		}
	}

	public void Trigger(IActionObject obj)
	{
		obj.Action();
	}

    public bool GetDown()
    {
        return down;
    }

	public void TriggerAll()
	{
		if (!down)
		{
			anim.SetTrigger("down");
			down = true;
			for(int i = 0; i < gameObjects.Length; i++)
			{
				Trigger(gameObjects[i].GetComponent<IActionObject>());
			}
		}
	}
}
