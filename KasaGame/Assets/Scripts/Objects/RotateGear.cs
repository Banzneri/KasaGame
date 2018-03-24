using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour {
    public GameObject player;
    public float riseAmount = 2;
    public Transform spawnPoint;

    public float rotateSpeed = 30f;

    public bool isActivated = false;

    private Vector3 riseLocation;
    private Vector3 originalLocation;
    public bool rising = false;
    [SerializeField] private Light _light;
    [SerializeField] private Light _spotLight;
    [SerializeField] private Material _activeMaterial;
    [SerializeField] private Material _inactiveMaterial;
    private float t = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        riseLocation = new Vector3(transform.position.x, transform.position.y + riseAmount, transform.position.z);
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
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
	}

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void Activate() {
        _light.gameObject.SetActive(true);
        _spotLight.gameObject.SetActive(true);
        rising = true;
        isActivated = true;
        gameObject.GetComponent<MeshRenderer>().material = _activeMaterial;
        rotateSpeed = rotateSpeed * 2;

        GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();
        GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SavePlayer();

        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();

        if (particle != null)
        {
            particle.Play();
        }
    }

}
