using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject CameraTarget;

    // Use this for initialization
    void Start () {
        ResetCameraPos();
    }
	
	// Update is called once per frame
	void Update () {

        // update camera
        UpdateCamera();
    }

    // Reset camera position behind player
    private void ResetCameraPos()
    {
        CameraTarget.transform.position = transform.position;
    }

    // Moves and turns camera
    private void UpdateCamera()
    {
        // orbit camera
        float mouseMovement = Input.GetAxis("Mouse X");
        CameraTarget.transform.Rotate(new Vector3(0, -mouseMovement, 0));

        // update Camera target position
        CameraTarget.transform.position = transform.position;
    }

}
