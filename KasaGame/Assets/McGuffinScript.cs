using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinScript : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    enum McGuffinOrigin { Level1, Level2, Level3 };

    [SerializeField]
    McGuffinOrigin mcGuffinOrigin;

	// Use this for initialization
	void Start () {
        if (mcGuffinOrigin == McGuffinOrigin.Level1) {
            GetComponent<Renderer>().material.color = Color.red;
        } else if(mcGuffinOrigin == McGuffinOrigin.Level2) {
            GetComponent<Renderer>().material.color = Color.green;
        } else {
            GetComponent<Renderer>().material.color = Color.blue;
        }
	}
	
	// Update is called once per frame
	void Update () {
        RotateMcGuffin(rotationSpeed);

        transform.Translate(Vector3.forward * Mathf.Sin(Time.time) / 200);
	}

    void RotateMcGuffin(float speed)
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
