using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour {
    public bool rotating = false;
    public GameObject player;
    public float riseAmount = 2;

    private Vector3 riseLocation;
    private Vector3 originalLocation;
    private bool rising = false;
    private Light light;
    private float t = 0f;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
        riseLocation = new Vector3(transform.position.x, transform.position.y + riseAmount, transform.position.z);
    }

    // Update is called once per frame
    void Update () {
        if (rotating)
        {
            transform.Rotate(0, 0, 3f);
        }
        if (rising)
        {
            transform.position = Vector3.Lerp(transform.position, riseLocation, 0.01f);
            light.GetComponent<VolumetricLight>().ScatteringCoef = Mathf.Lerp(0, 0.2f, t);
            t += 0.5f * Time.deltaTime;

            if (transform.position == riseLocation)
            {
                rising = false;
            }
        }
        Interact();
	}

    void Interact()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            Debug.Log("Press e");
            if (Input.GetKeyDown(KeyCode.E))
            {
                light.enabled = true;
                rotating = true;
                rising = true;
            }
        }
    }

}
