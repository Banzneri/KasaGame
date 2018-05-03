using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
	[SerializeField] ParticleSystem _destroyParticles;
	[SerializeField] ParticleSystem _moveParticles;
	[SerializeField] AudioSource _moveSound;
	[SerializeField] AudioSource _deathSound;

	private Vector3 _startPos = Vector3.zero;

	private bool _destroyed = false;

	private void Start() {
		_startPos = transform.position;
	}

	private void Update() {
		Vector3 pos = transform.position;
		pos.y = pos.y - transform.lossyScale.y;
		if (!_destroyed)
		{
			_moveParticles.transform.position = pos;
			_moveParticles.transform.forward = GetComponent<Rigidbody>().velocity * -1f;	
		}
		HandleDestruction();
		//_moveParticles.transform.position = _startPos;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.transform.tag == "Player")
		{
			other.gameObject.GetComponent<MyCharManager>().TakeDamage();
			DestroyRock();
		}
		else if (other.transform.tag != "Rock" && !_destroyed)
		{
			_moveSound.Play();
			_moveParticles.Play();
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.transform.tag != "Player" && other.transform.tag != "Rock")
		{
			_moveSound.Stop();
			_deathSound.Play();
			_moveParticles.Stop();
		}
	}

	public void DestroyRock()
	{
		_destroyed = true;
		GetComponent<Renderer>().enabled = false;
		_destroyParticles.Play();
		Destroy(_moveParticles);
		Destroy(GetComponent<Rigidbody>());
		Destroy(GetComponent<Collider>());
	}

	private void HandleDestruction()
	{
		if (_destroyed)
		{
			if (!_destroyParticles.isPlaying)
			{
				Destroy(gameObject);
			}
		}
	}
}
