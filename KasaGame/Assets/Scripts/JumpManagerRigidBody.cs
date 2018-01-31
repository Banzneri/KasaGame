using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManagerRigidBody : MonoBehaviour
{
    public float jumpPower = 10f;

    private bool jumping = false;
    private bool grounded = false;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumping = false;
            grounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bottomOfPlayer = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y / 2, transform.position.z);
        if (!Physics.Raycast(bottomOfPlayer, Vector3.down, 0.3f) && grounded)
        {
            Debug.Log("grounded");
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            grounded = false;
            jumping = true;
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            Debug.Log("Jumping");
        }

        if (!grounded)
        {
            
        }
    }
}
