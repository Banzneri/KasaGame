using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    private bool inTrigger;
    public Door doorReference;

    private AudioSource audio;

    public bool isPickedUp = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();    
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
            if (!audio.isPlaying && !isPickedUp)
            {
                audio.Play();   
            }
            doorReference.doorKey = true;
            isPickedUp = true;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        if (isPickedUp && !audio.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
