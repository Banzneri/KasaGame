using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour {
    public bool rotating = true;
    public GameObject player;

    // Update is called once per frame
    void Update () {
        if (rotating)
        {
            transform.Rotate(0, 0, 3f);
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
                rotating = !rotating;
            }
        }
        else
        {
            Debug.Log("");
        }
    }

}
