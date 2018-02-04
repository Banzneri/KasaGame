using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float angularSpeed;
    public float maxCameraHeight = 12f;
    public float minCameraHeight = 2f;
    public float cameraDistance = 20f;
    public float minCameraDistance = 12f;
    public float maxCameraDistance = 20f;
    public float zoomSpeed = 10;

    private bool blocked = false;
    private Vector3 start;
    private Vector3 end;

    [SerializeField]
    [HideInInspector]
    private Vector3 initialOffset;

    private Vector3 currentOffset;

    [ContextMenu("Set Current Offset")]
    private void SetCurrentOffset()
    {
        if (target == null)
        {
            return;
        }

        initialOffset = transform.position - target.position;
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Assign a target for the camera in Unity's inspector");
        }

        currentOffset = initialOffset;
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 lookAt = new Vector3(target.position.x, target.position.y + 1, target.position.z);
        transform.position = lookAt + currentOffset;

        float movementX = Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;
        float movementY = Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
        float movementXController = Input.GetAxis("RightX") * angularSpeed * Time.deltaTime;
        float movementYController = Input.GetAxis("RightY") * angularSpeed * Time.deltaTime;
        float trueX = movementX != 0 ? movementX : movementXController;
        float trueY = movementY != 0 ? movementY : movementYController;

        if (!Mathf.Approximately(trueX, 0f) || !Mathf.Approximately(trueY, 0f))
        {
            transform.RotateAround(lookAt, Vector3.up, trueX);
            Vector3 direction = new Vector3(transform.right.x, Vector3.left.y, transform.right.z);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            float xAngle = transform.eulerAngles.x;
            float yAngle = transform.eulerAngles.y;

            if (!(xAngle < 2 && trueY > 0) && !(xAngle > 50 && trueY < 0))
            {
                transform.RotateAround(lookAt, -direction, trueY);
                if (xAngle < 2 || xAngle > 300)
                {
                    Debug.Log("AAA");
                    transform.rotation = Quaternion.Euler(2, yAngle, 0);
                }
                else if (xAngle > 50)
                {
                    transform.rotation = Quaternion.Euler(50, yAngle, 0);
                }
            }

            if (transform.position.y < target.position.y + 1)
            {
                transform.position = new Vector3(transform.position.x, target.position.y + 1, transform.position.z);
            }

            transform.LookAt(lookAt);
            currentOffset = transform.position - lookAt;
        }
        HandleObstacles();
        HandleZooming();
    }

    void HandleObstacles()
    {
        RaycastHit hit;
        LayerMask obstaclesMask = LayerMask.GetMask("Obstacles");

        if (Physics.Linecast(target.position, transform.position, out hit, obstaclesMask))
        {
            transform.position = hit.point;
        }
        else
        {
            currentOffset = Vector3.Lerp(currentOffset, currentOffset.normalized * cameraDistance, 0.02f);
        }
    }

    void HandleZooming()
    {
        float zoom = (-Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomSpeed + cameraDistance;

        if (zoom < maxCameraDistance && zoom > minCameraDistance)
        {
            cameraDistance = zoom;
        }

    }
}
