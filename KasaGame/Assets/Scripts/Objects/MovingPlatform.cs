using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActionObject {
	[SerializeField] private Transform startLocation;
	[SerializeField] private Transform endLocation;
	[SerializeField] private float speed;
	[SerializeField] private float waitTime;
    [SerializeField] private bool automatic;
    private bool goingToEndLoc = true; //Used only if the platform is not automatic.
    public ParticleSystem splasher;
    [SerializeField] Screw[] automaticScrews;

    private Vector3 currentDestination;
	private float waitCounter = 0;

	// Use this for initialization
	void Start () {
        currentDestination = new Vector3(startLocation.position.x, startLocation.position.y, startLocation.position.z);
	}
	
    void MoveToVector3(Vector3 target)
    {
        transform.position = target;
    }

    void SetToAutomatic()
    {
        automatic = true;
    }

    bool CheckAutomationArray()
    {
        bool b = false;
        int screwsDown = 0;

        foreach (Screw screw in automaticScrews) {
			if (screw.GetDown()) {
				screwsDown++;
			}
			if (screwsDown >= automaticScrews.Length) {
				b = true;
			}
			//Debug.Log("Screws down: " + screwsDown);
        }

        return b;
    }

    // Update is called once per frame
    void FixedUpdate () {
		Vector3 trajectory = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);

        MoveToVector3(trajectory);

        if(automaticScrews.Length > 0) {
            //Debug.Log("Screw length is higher than 0");
            if (CheckAutomationArray()) {
                SetToAutomatic();
            }
        }

		if (transform.position == startLocation.position && automatic)
		{
			if (waitCounter == 0)
			{
				GetComponent<AudioSource>().Stop();
			}
			waitCounter += Time.deltaTime;
			if (waitCounter > waitTime)
			{
				currentDestination = endLocation.position;
				waitCounter = 0;
				GetComponent<AudioSource>().Play();
			}
		} 
		else if (transform.position == endLocation.position && automatic)
		{
			if (waitCounter == 0)
			{
				GetComponent<AudioSource>().Stop();
			}
			waitCounter += Time.deltaTime;
			if (waitCounter > waitTime)
			{
				currentDestination = startLocation.position;
				waitCounter = 0;
				GetComponent<AudioSource>().Play();
			}
		}

        //Stop sound for non-automatic
        if (transform.position == currentDestination)
        {
            GetComponent<AudioSource>().Stop();
        }

        if (splasher != null && !splasher.isPlaying && transform.position != currentDestination)
        {
            splasher.Play();
        }
        else if (splasher != null && transform.position == currentDestination)
        {
            splasher.Stop();
        }
    }

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.transform.parent = transform;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.transform.parent = null;
		}		
	}

    public void Action()
    {
        if (goingToEndLoc && !automatic)
        { 
                currentDestination = endLocation.position;
                goingToEndLoc = false;
                GetComponent<AudioSource>().Play(); 
        }
        else if (!goingToEndLoc && !automatic)
        {
                currentDestination = startLocation.position;
                goingToEndLoc = true;
                GetComponent<AudioSource>().Play();
        }
    }
}
