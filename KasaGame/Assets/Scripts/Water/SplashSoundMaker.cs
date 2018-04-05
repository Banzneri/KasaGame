using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSoundMaker : MonoBehaviour {

    private AudioSource _splash;

	// Use this for initialization
	void Start () {
        _splash = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(!_splash.isPlaying && other.tag == "Player")
        {
            _splash.Play();
        }
    }
}
