using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {



    // Update is called once per frame
    void Update () {

        if(transform.position.x > -75)
        {
            transform.position += new Vector3(-1f, 0, 0) * 20f * Time.deltaTime;
        }
		else
        {
            transform.position += new Vector3(0, -1f, 0) * 30f * Time.deltaTime;
            transform.position += new Vector3(-1, 0, 0f) * 15f * Time.deltaTime;
            if (transform.position.y <= -15)
            {
                Destroy(gameObject);
            }
        }
	}
}
