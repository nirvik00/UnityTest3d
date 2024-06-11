// use cinemachine, create camera_system empty game object, add this script to camera_system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraSystem : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;

    //
    [SerializeField] private bool useEdgeScrolling = false;

    // 
    private bool dragPanModeActive = false;
    private Vector2 lastMousePosition;

    //
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float targetFieldOfView = 50f;
    [SerializeField] private float fieldOfViewMax = 100f;
    [SerializeField] private float fieldOfViewMin = 10f;

    //
    private Vector3 followOffset;
    [SerializeField] private float followOffsetMin = 5f;
    [SerializeField] private float followOffsetMax = 50;

    private void Awake()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        HandleCameraMovement();
        // HandleCameraMovementEdgeScrolling();
        HandleCameraMovementDragPan();
        HandleCameraRotation();
        HandleCameraZoom_FOV();
        // HandleCameraZoom_LowerY();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputDir.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDir.x = 1f;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            inputDir.y = 1f;
        }
        if (Input.GetKey(KeyCode.C))
        {
            inputDir.y = -1f;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x + transform.up * inputDir.y;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (useEdgeScrolling == true)
        {
            int edgeSCrollSize = 20;
            if (Input.mousePosition.x < edgeSCrollSize)
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.x > Screen.width - edgeSCrollSize)
            {
                inputDir.x = +1f;
            }
            if (Input.mousePosition.y < edgeSCrollSize)
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.x > Screen.height - edgeSCrollSize)
            {
                inputDir.z = +1f;
            }
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetMouseButtonDown(0))
        {
            dragPanModeActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragPanModeActive = false;
        }

        if (dragPanModeActive == true)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 1f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) { rotateDir = +1f; }
        if (Input.GetKey(KeyCode.E)) { rotateDir = -1f; }

        float rotateSpeed = 100f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleCameraZoom_FOV()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView += 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView -= 5;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

        float moveSpeed = 10f;
        // Debug.Log(cinemachineVirtualCamera.m_Lens.FieldOfView);

        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView,
        targetFieldOfView, Time.deltaTime * moveSpeed);
    }

    private void HandleCameraZoom_LowerY()
    {
        Vector3 zoomDir = followOffset.normalized;
        float zoomamount = 3f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset += zoomDir * zoomamount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset += zoomDir * zoomamount;
        }
        if (followOffset.magnitude < followOffsetMin)
        {
            followOffset = zoomDir * followOffsetMin;
        }
        if (followOffset.magnitude > followOffsetMax)
        {
            followOffset = zoomDir * followOffsetMax;
        }

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        // cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffset;
    }

}
