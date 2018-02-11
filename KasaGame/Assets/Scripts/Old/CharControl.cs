using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedWhileFalling = 2f;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float gravity = 0.5f;

    public Animator animator;

    private Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        float curSpeed = controller.isGrounded ? speed : speedWhileFalling;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0f;
            moveDirection *= speed;

            animator.SetFloat("MoveSpeed", moveDirection.magnitude);

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            float y = moveDirection.y;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = y;
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }

        if (moveDirection.magnitude != 0)
        {
            Vector3 dir = new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z);
            transform.forward = dir;
        }

        moveDirection.y -= gravity;
        Vector2 horizontalMove = new Vector2(moveDirection.x, moveDirection.z);

        if (-Mathf.Abs(horizontalMove.magnitude) < moveDirection.y && 
            moveDirection.y < 0 && controller.isGrounded)
        {
            moveDirection.y = -Mathf.Abs(horizontalMove.magnitude) - 0.001f;
        }

        controller.Move(moveDirection * Time.deltaTime);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void UpdateAnimator(CharacterController controller)
    {
        
    }

}
