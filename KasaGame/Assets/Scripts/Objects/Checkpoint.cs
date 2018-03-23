using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	[SerializeField] private Material _activeMaterial;
	[SerializeField] private Material _inactiveMaterial;
	[SerializeField] private bool _isActivated = false;
	[SerializeField] private GameObject[] _sides;
	[SerializeField] private GameObject _gear;

	private GameObject _player;
	// Use this for initialization
	void Start () {
		SetMaterials();
		_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
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
        if (Vector3.Distance(transform.position, _player.transform.position) < 5)
        {
            //Debug.Log("Press e");
            if (!_isActivated)
            {
                Activate();
				SetMaterials();
            }
        }
    }

	void Activate ()
	{
		_gear.GetComponent<RotateGear>().Activate();
		_gear.GetComponent<AudioSource>().Play();
		_isActivated = true;
	}
}
