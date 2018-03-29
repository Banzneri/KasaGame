using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPusher : MonoBehaviour {

    public float pushPower = 5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // If player walks towards a box, it's pushed
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        //Prevents pushing objects that are below the character
        if (hit.moveDirection.y < -0.3F && hit.gameObject.tag == "Box")
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //Forces the box move purely on x or z axis
        if (Mathf.Abs(pushDir.x) > Mathf.Abs(pushDir.z))
        {
            pushDir.Set(pushDir.x, 0, 0);
        }
        else
        {
            pushDir.Set(0, 0, pushDir.z);
        }

        body.velocity = pushDir * pushPower;
    }
}
