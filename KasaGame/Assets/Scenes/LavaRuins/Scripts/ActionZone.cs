using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZone : MonoBehaviour, ITriggerObject<IActionObject> {

	[SerializeField]
	public GameObject[] actionObjects;

	[SerializeField]
	private bool OnceOnly;

	private bool _Used;

	private void Start() {
		_Used = false;
	}

	public void Trigger(IActionObject actionObject)
	{
		actionObject.Action();
	}

	public void TriggerAll(){

		if (OnceOnly) {
			_Used = true;
		}

		for(int i = 0; i < actionObjects.Length; i++)
		{
			Trigger(actionObjects[i].GetComponent<IActionObject>());
		}
	}

	public void OnTriggerEnter(Collider other){

		if (other.CompareTag ("Player") && !_Used) {
			TriggerAll ();
		}
	}

}
