using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzleManager : MonoBehaviour
{
    public GameObject myLight;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            myLight.GetComponent<Light>().enabled = true;
            Debug.Log("Player");
        }
    }
}
