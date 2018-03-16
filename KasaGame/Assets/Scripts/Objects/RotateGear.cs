using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour, IActionObject, ITriggerObject<IActionObject> {
    public GameObject player;
    public float riseAmount = 2;
    public Transform spawnPoint;

    public float rotateSpeed = 30f;

    public bool isActivated = false;

    private Vector3 riseLocation;
    private Vector3 originalLocation;
    public bool rising = false;
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
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        if (rising)
        {
            transform.position = Vector3.Lerp(transform.position, riseLocation, Time.deltaTime);
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
            if (!isActivated)
            {
                Activate();
            }
        }
    }

    public void Action()
    {
        Activate();
    }

    public void Trigger(IActionObject obj)
    {
    }

    public void TriggerAll()
    {
        Action();
    }

    public void Activate() {
        _light.enabled = true;
        rising = true;
        isActivated = true;

        GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();

        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();

        if (particle != null)
        {
            particle.Play();
        }
    }

}
