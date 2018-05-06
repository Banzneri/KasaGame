using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour, ITriggerObject<IActionObject> {
	[SerializeField] private float _waitTimeInSeconds = 2f;
	[SerializeField] private float _flameTime = 2f;
	[SerializeField] GameObject _actionObject;

	private bool _flaming = false;
	private float _waitCounter = 0f;
	private float _flameCounter = 0f;

	ParticleSystem[] _particles;

	// Use this for initialization
	void Start () {
		_particles = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_flaming)
		{
			_waitCounter += Time.deltaTime;

			if (_waitCounter > _waitTimeInSeconds)
			{
				StartFlaming();
			}	
		}
		else
		{
			_flameCounter += Time.deltaTime;
			if (_flameCounter > _flameTime)
			{
				StopFlaming();
			}
		}
	}

	void StartFlaming()
	{
		_flaming = true;
		_waitCounter = 0f;
		Trigger(_actionObject.GetComponent<IActionObject>());
		foreach (ParticleSystem particle in _particles)
		{
			particle.Play();
		}
	}

	void StopFlaming()
	{
		Trigger(_actionObject.GetComponent<IActionObject>());
		_flaming = false;
		_flameCounter = 0f;
		foreach (ParticleSystem particle in _particles)
		{
			particle.Stop();
		}
	}

	public void Trigger(IActionObject obj)
	{
		obj.Action();
	}

	public void TriggerAll()
	{

	}
}
