using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKai : MonoBehaviour, IActionObject {
	[SerializeField] private Transform startLocation;
	[SerializeField] private Transform endLocation;
	[SerializeField] private float speed;
	[SerializeField] private float waitTime;
    [SerializeField] private bool automatic;
    private GameObject _player;
    public bool goingToEndLoc = true; //Used only if the platform is not automatic.
    public ParticleSystem splasher;
    [SerializeField] Screw[] automaticScrews;

    private Vector3 currentDestination;
	private float waitCounter = 0;
	private float minDistance = 0.1f;
    public Vector3 savedLocation = Vector3.zero;

	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startLoc = goingToEndLoc ? startLocation.position : endLocation.position;
        currentDestination = startLoc;
        transform.position = startLoc;
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

        if(_player.GetComponent<MyCharManager>().Health <= 0)
        {
            currentDestination = startLocation.position;
            goingToEndLoc = true;
        }

		Vector3 trajectory = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);

        MoveToVector3(trajectory);

        if(automaticScrews.Length > 0) {
            //Debug.Log("Screw length is higher than 0");
            if (CheckAutomationArray()) {
                SetToAutomatic();
            }
        }

		if (Vector3.Distance(transform.position, startLocation.position) < minDistance && automatic)
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
		else if (Vector3.Distance(transform.position, endLocation.position) < minDistance && automatic)
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
