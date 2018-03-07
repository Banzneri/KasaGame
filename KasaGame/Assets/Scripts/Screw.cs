using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour, ITriggerObject<IActionObject> {
	private Animator anim;
	private bool down = false;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Trigger(IActionObject obj)
	{
		
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
		}
	}
}
