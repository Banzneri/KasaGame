using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour {

    private Transform player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        Debug.Log(distanceToPlayer);

        if (Input.GetButton("Grab") && distanceToPlayer < 1.5)
        {
            transform.parent = player;
        }
        else
        {
            transform.parent = null;
        }

        ///////////////////////


	}
}
