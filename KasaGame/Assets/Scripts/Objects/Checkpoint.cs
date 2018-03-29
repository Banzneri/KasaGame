using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	[SerializeField] private Material _activeMaterial;
	[SerializeField] private Material _inactiveMaterial;
	[SerializeField] private bool _isActivated = false;
	[SerializeField] private GameObject[] _sides;
	[SerializeField] private GameObject _gear;
	[SerializeField] private float _actDistance = 5.0f;

	private GameObject _player;

	bool WithinDistance {
		get {
			return Vector3.Distance(transform.position, _player.transform.position) < _actDistance;
		}
	}

	void Start () {
		SetMaterials();
		_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		Interact ();
	}

	void SetMaterials ()
	{
		foreach (var side in _sides)
		{
			if (_isActivated)
			{
				side.GetComponent<MeshRenderer>().material = _activeMaterial;
			}
			else
			{
				side.GetComponent<MeshRenderer>().material = _inactiveMaterial;
			}
		}
	}

	void Interact()
    {
        if (WithinDistance)
        {
            if (!_isActivated)
            {
                Activate();
            }
        }
    }

	void Activate ()
	{
		_gear.GetComponent<RotateGear>().Activate();
		_gear.GetComponent<AudioSource>().Play();
		_isActivated = true;
		SetMaterials();
	}
}
