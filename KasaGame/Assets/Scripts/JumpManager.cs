using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour {
    public float jumpPower = 10f;
    public float gravity = 0.1f;
    public Animator animator;

    private float yVelocity = 0f;

    private bool jumping = false;
    private bool grounded = false;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("OnGround");
            animator.SetBool("Grounded", true);
            jumping = false;
            grounded = true;
            yVelocity = 0f;
        }
    }

    // Update is called once per frame
    void Update () {
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
            yVelocity = jumpPower;
            Debug.Log("Jumping");
        }

        if (!grounded)
        {
            yVelocity -= gravity;
            transform.position += new Vector3(0, yVelocity * Time.deltaTime, 0);
        }
        animator.SetBool("Grounded", grounded);
    }
}
