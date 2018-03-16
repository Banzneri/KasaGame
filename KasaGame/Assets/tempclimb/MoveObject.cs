using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    [SerializeField]
    private float RotateX;

    [SerializeField]
    private float RotateY;

    [SerializeField]
    private float RotateZ;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(RotateX * Time.deltaTime, RotateY * Time.deltaTime, RotateZ * Time.deltaTime));

	}
}
