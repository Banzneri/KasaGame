using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeldaPuzzleManager : MonoBehaviour {
    [SerializeField] private float turnSpeed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float rotateHorizontal = Input.GetAxis("Horizontal");
        float rotateVertical = Input.GetAxis("Vertical");

        transform.Rotate(-Vector3.forward * rotateVertical * turnSpeed * Time.deltaTime);
        transform.Rotate(-Vector3.right * rotateHorizontal * turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }
}
