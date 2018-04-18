using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour, IActionObject {

	[SerializeField]
	private GameObject[] Objects;

	[SerializeField]
	private bool WakeUpOnly;

	public void Action(){
		
		for (int i = 0; i < Objects.Length; i++) {

			if (WakeUpOnly) {
				Objects [i].SetActive(true);
			} else {
				Objects [i].SetActive(!Objects[i].activeSelf);
			}
		}
	}

}
