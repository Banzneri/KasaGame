using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDropper : MonoBehaviour {
	public float _waitTimeInSeconds = 3;
	public GameObject _rock;
	
	private float _waitCounter = 0.0f;
	
	// Update is called once per frame
	void Update () {
		_waitCounter += Time.deltaTime;

		if (_waitCounter > _waitTimeInSeconds)
		{
			Instantiate(_rock, transform.position, Quaternion.Euler(Vector3.zero));
			_waitCounter = 0.0f;
		}
	}
}
