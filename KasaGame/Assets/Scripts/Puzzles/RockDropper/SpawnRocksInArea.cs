using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocksInArea : MonoBehaviour {
	public float _waitTimeInSeconds = 3;
	public GameObject _rock;

	private float _waitCounter = 0.0f;
	
	// Update is called once per frame
	void Update () {
		_waitCounter += Time.deltaTime;

		if (_waitCounter > _waitTimeInSeconds)
		{
			float randX = Random.Range(transform.position.x - transform.lossyScale.x * 5, transform.position.x + transform.lossyScale.x * 5);
			float randZ = Random.Range(transform.position.z - transform.lossyScale.z * 5, transform.position.z + transform.lossyScale.z * 5);
			Vector3 pos = new Vector3(randX, transform.position.y, randZ);
			Instantiate(_rock, pos, Quaternion.Euler(Vector3.zero));
			_waitCounter = 0.0f;
		}
	}
}
