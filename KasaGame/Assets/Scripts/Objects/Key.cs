using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    private bool inTrigger;
    public Door doorReference;

    private AudioSource _audio;

    public bool isPickedUp = false;

    void Start()
    {
        _audio = GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    private void Update()
    {
        if (inTrigger)
        {
            if (!_audio.isPlaying && !isPickedUp)
            {
                _audio.Play();   
            }
            doorReference.doorKey = true;
            GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>().PickupKey();
            isPickedUp = true;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        if (isPickedUp && !_audio.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
