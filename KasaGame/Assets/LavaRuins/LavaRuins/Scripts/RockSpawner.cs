using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour {

	#region Settings

	// Rock prefab
	[SerializeField]
	private GameObject _RockPrefab;

	// Spawner starts spawning after this delay
	[SerializeField]
	private float _InitialDelay;

	// Rate of spawning
	[SerializeField]
	private float _SpawnRate;

	#endregion

	#region Private Variables

	// Spawned rocks
	private List<GameObject> _Rocks;

	// Keeps count of time
	private float _Timer;

	// Initial delay used
	private bool _InitialDone;

	#endregion


	// Use this for initialization
	void Start () {
		_Timer = 0;
		_InitialDone = false;
		_Rocks = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!_InitialDone) {
			_Timer += Time.deltaTime;
			if (_Timer >= _InitialDelay) {
				_InitialDone = true;
				Spawn ();
			}
		} else {
			_Timer += Time.deltaTime;
			if (_Timer >= _SpawnRate) {
				Spawn ();
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(_Rocks.Contains(other.gameObject)){
			_Rocks.Remove (other.gameObject);
			Destroy(other.gameObject);

		}
	}

	// Spawns rock, resets timer
	private void Spawn() {
		_Timer = 0;
		GameObject NewRock = Instantiate (_RockPrefab, transform.position, transform.rotation);
		NewRock.transform.parent = this.transform;
		_Rocks.Add (NewRock);
	}

	// Draw spawn point
	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube (transform.position, new Vector3 (3, 3, 3));
	}

}
