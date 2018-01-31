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

    private void LateUpdate()
    {
        transform.position = target.position + currentOffset;

        float movementX = Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;
        float movementY = Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
        float movementXController = Input.GetAxis("RightX") * angularSpeed * Time.deltaTime;
        float movementYController = Input.GetAxis("RightY") * angularSpeed * Time.deltaTime;
        float trueX = movementX != 0 ? movementX : movementXController;
        float trueY = movementY != 0 ? movementY : movementYController;

        if (!Mathf.Approximately(trueX, 0f) || !Mathf.Approximately(trueY, 0f))
        {
            Vector3 direction = new Vector3(transform.right.x, Vector3.left.y, transform.right.z);
            transform.RotateAround(target.position, Vector3.up, trueX);
            float nextY = transform.position.y - trueY;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            bool yRotateLocked = nextY < minCameraHeight + target.transform.position.y || nextY > maxCameraHeight + target.transform.position.y;

            if (!yRotateLocked)
            {
                transform.RotateAround(target.position, -direction, trueY);
            }

            currentOffset = transform.position - target.position;
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
        Debug.Log(zoom);

        if (zoom < maxCameraDistance && zoom > minCameraDistance)
        {
            cameraDistance = zoom;
        }

    }
}
