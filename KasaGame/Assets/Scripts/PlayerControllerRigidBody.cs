using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRigidBody : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed = 10f;
    public float turnSpeed = 0.1f;
    public float runSpeed = 15f;

    Vector3 orderedPosition;
    Vector3 velocity;

    bool mouseDown = false;
    bool movingForward = false;
    bool movingBackward = false;
    bool movingLeft = false;
    bool movingRight = false;
    bool isRunning = false;

    void Awake()
    {
        orderedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardMovement();
    }

    private void HandleKeyboardMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            movingForward = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            movingForward = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movingBackward = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            movingBackward = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movingLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            movingLeft = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            movingRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            movingRight = false;
        }

        if (movingForward)
        {
            Vector3 direction = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
        }
        else if (movingBackward)
        {
            Vector3 direction = new Vector3(-cam.transform.forward.x, 0, -cam.transform.forward.z);
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
        }

        if (movingLeft)
        {
            Vector3 direction = new Vector3(-cam.transform.right.x, 0, -cam.transform.right.z);
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
        }
        else if (movingRight)
        {
            Vector3 direction = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
        }

        if (IsMoving())
        {
            if (true)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    isRunning = true;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    isRunning = false;
                }
            }
            transform.position += transform.forward * Time.deltaTime * (isRunning ? runSpeed : moveSpeed);
        }
    }

    private void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }


        if (mouseDown)
        {
            LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask layerMask1 = 1 << LayerMask.NameToLayer("Obstacles");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask) && !Physics.Raycast(ray, out hit, 1000, layerMask1))
            {
                orderedPosition = new Vector3(hit.point.x, 2, hit.point.z);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, orderedPosition, 0.2f);
    }

    private bool IsMoving()
    {
        return movingForward || movingBackward || movingLeft || movingRight;
    }
}
