using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour {

    private GameObject _player;
    private GameObject _particleObj;
    private ParticleSystem _particleSys;
    private AudioSource _splashIn;

	// Use this for initialization
	void Start () {
        _splashIn = GetComponent<AudioSource>();
		_player = GameObject.FindGameObjectWithTag("Player");
        _particleObj = transform.FindChild("ParticleSystem").gameObject;
        _particleSys = _particleObj.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && _player.GetComponent<Rigidbody>().velocity.y < 0)
        {
            _splashIn.Play();
            _particleObj.transform.SetPositionAndRotation(_player.transform.position, _particleObj.transform.rotation);
            _particleSys.Play();
        }
    }

    // Update is called once per frame
    void Update () {
	}
}
