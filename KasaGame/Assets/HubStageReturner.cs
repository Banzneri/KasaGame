using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubStageReturner : MonoBehaviour {

    [SerializeField]
    float triggerYLevel = -15;

    [SerializeField]
    Transform returnPoint;

    [SerializeField]
    Light stageLights;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y <= triggerYLevel) {
            stageLights.shadowStrength -= 1 * Time.deltaTime;
            if(stageLights.shadowStrength <= 0) {
                transform.position = returnPoint.position;
                stageLights.shadowStrength = 1;
            }
        }
	}
}
