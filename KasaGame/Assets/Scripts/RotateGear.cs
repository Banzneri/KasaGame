using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour {
    public GameObject player;
    public float riseAmount = 2;
    public Transform spawnPoint;

    public bool isActivated = false;

    private Vector3 riseLocation;
    private Vector3 originalLocation;
    private bool rising = false;
    private Light _light;
    private float t = 0f;

    private void Start()
    {
        _light = GetComponentInChildren<Light>();
        player = GameObject.FindGameObjectWithTag("Player");
        riseLocation = new Vector3(transform.position.x, transform.position.y + riseAmount, transform.position.z);
    }

    // Update is called once per frame
    void Update () {
        if (isActivated)
        {
            if (!rising)
            {
                Activate();
            }
            transform.Rotate(0, 0, 3f);
        }
        if (rising)
        {
            transform.position = Vector3.Lerp(transform.position, riseLocation, 0.01f);
            _light.GetComponent<VolumetricLight>().ScatteringCoef = Mathf.Lerp(0, 1f, t);
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
            //Debug.Log("Press e");
            if (Input.GetKeyDown(KeyCode.E) && !isActivated)
            {
                _light.enabled = true;
                rising = true;
                isActivated = true;
            }
        }
    }

    void Activate() {
        _light.enabled = true;
        rising = true;
        isActivated = true;
    }

}
