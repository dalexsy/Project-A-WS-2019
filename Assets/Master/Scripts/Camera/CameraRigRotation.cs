using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraRigRotation : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float rotationSpeed = 1f;
    [SerializeField] private AnimationCurve animationCurve = null;
    [SerializeField] private GameObject[] planks;
    private InputManager inputManager;
    private Camera mainCamera;
    private float orthoZoomSpeed = 0.01f;
    private bool isRotating = false;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        mainCamera = Camera.main;

        // Find all Planks in scene
        planks = GameObject.FindGameObjectsWithTag("Plank");

        // Sort Planks alphabetically
        Array.Sort(planks, (x, y) => String.Compare(x.transform.name, y.transform.name));
    }

    private void Update()
    {
        // Create new list of Plank positions
        List<Vector3> plankPositions = new List<Vector3>();

        // Add each Plank's position to plankPositions
        foreach (var plank in planks)
            plankPositions.Add(plank.transform.position);

        // Return average position of all Planks
        var averagePosition = GetMeanVector(plankPositions);

        // Set rig's position to average Plank position
        this.transform.position = averagePosition;

        if (!inputManager.isUsingTouch && isRotating == false) MouseRotation();
    }

    void LateUpdate()
    {
        if (inputManager.isUsingTouch)
        {
            float pinchAmount = 0;

            float yRotation = transform.eulerAngles.y;

            DetectTouchMovement.Calculate();

            if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
            {
                pinchAmount = DetectTouchMovement.pinchDistanceDelta;
            }

            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
            {
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
            }

            // ... change the orthographic size based on the change in distance between the touches.
            mainCamera.orthographicSize += pinchAmount * orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 5f);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation + pinchAmount, transform.eulerAngles.z);
        }
    }
    private void MouseRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float moveY = Input.GetAxis("Mouse X");

            // Set input buffer to prevent input oversensitivity
            float inputBuffer = Screen.height * .01f * Mathf.Sign(moveY);

            if (moveY < inputBuffer && moveY != 0) StartCoroutine(RotateRig(1));
            if (moveY > inputBuffer && moveY != 0) StartCoroutine(RotateRig(-1));
        }
    }

    // Coroutine to rotate camera rig
    IEnumerator RotateRig(int direction)
    {
        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // Set target rotation to 90 degrees around rig's y-axis in given direction
        Vector3 targetRotation = new Vector3(0, 90f * direction, 0);

        // Set start rotation as rig's current rotation
        Quaternion startRotation = transform.rotation;

        // Set end rotation as start rotation plus target rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(targetRotation);

        // Reset time
        float t = 0f;

        // While running
        while (t < 1f)
        {
            // Increase time by rotation speed
            t += Time.deltaTime * rotationSpeed;

            // Rotate towards end rotation using animation curve
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, animationCurve.Evaluate(t));

            // Return to top of while loop
            yield return null;
        }

        this.isRotating = false;
        yield return null;
    }

    // Returns mean position of list of vectors
    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        // If list is empty, return (0,0,0)
        if (positions.Count == 0) return Vector3.zero;

        // Reset meanVector
        Vector3 meanVector = Vector3.zero;

        // Add each position to meanVector
        foreach (Vector3 pos in positions)
            meanVector += pos;

        // Return meanVector divided by number of positions
        return (meanVector / positions.Count);
    }
}
