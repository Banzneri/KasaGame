using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimberController : MonoBehaviour {

    public GameObject CameraTarget;
    public CharacterController Controller;

    private float _RunSpeed = 5;
    private Vector3 _JumpForce;

    public void Update()
    {
        // walk and jump
        Jump();
        MovePlayer();

        // add gravity
        if(!Controller.isGrounded)
        {
            Controller.Move(new Vector3(0, -20 * Time.deltaTime, 0));
        }

    }

    // Moves player
    private void MovePlayer()
    {

        // directions and volumes of moving
        float movementZ = 0;
        float movementX = 0;

        // up and down
        if (Input.GetKey(KeyCode.W))
        {
            movementZ = 1;
        } else if(Input.GetKey(KeyCode.S))
        {
            movementZ = -1;
        }

        // left and right
        if (Input.GetKey(KeyCode.A))
        {
            movementX = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementX = 1;
        }

        // Camera vectors
        Vector3 forward = CameraTarget.transform.forward;
        Vector3 right = CameraTarget.transform.right;

        // Normalize
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // desired direction
        Vector3 finalMovement = forward * movementZ + right * movementX;
        _JumpForce.x = finalMovement.x * Time.deltaTime * _RunSpeed;
        _JumpForce.z = finalMovement.z * Time.deltaTime * _RunSpeed;

        // move and rotate player
        if(!finalMovement.Equals(new Vector3(0, 0, 0)))
        {
            Controller.Move(finalMovement * Time.deltaTime * _RunSpeed);

            Quaternion rotation = Quaternion.LookRotation(finalMovement);
            transform.rotation = rotation;
        }
    }

    // Jump
    private void Jump()
    {
        // if player is on ground and presses space bar
        if(Controller.isGrounded)
        {
            _JumpForce.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _JumpForce.y = 40f;
            }
        }
       

        // manage JumpForce
        if (_JumpForce.y > 0)
        {
            _JumpForce.y -= 45f * Time.deltaTime;
            Controller.Move(_JumpForce * Time.deltaTime);
        }
        else
        {
            _JumpForce = Vector3.zero;
        }
    }

}
