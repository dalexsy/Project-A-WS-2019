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
    private PlankManager plankManager;
    private Camera mainCamera;
    private float orthoZoomSpeed = -0.003f;
    private int turnDirection = 0;
    private bool isRotating = false;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
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
    }

    private void LateUpdate()
    {
        if (plankManager.hasReachedGoal)
        {
            RotateAroundLevel();
            return;
        }

        if (inputManager.isUsingTouch) TouchRotation();
        if (!inputManager.isUsingTouch && isRotating == false) MouseRotation();
    }

    private void FixedUpdate()
    {
        AngleCorrection();
    }

    private void RotateAroundLevel()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
    }

    private void TouchRotation()
    {
        if (Input.touchCount == 2)
        {
            inputManager.isDoubleSwiping = true;

            float pinchAmount = 0;
            float turnAmount = 0;
            float yRotation = transform.eulerAngles.y;

            DetectTouchMovement.Calculate();

            // Limit input sensitivity for zoom
            if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0.2f)
                pinchAmount = DetectTouchMovement.pinchDistanceDelta;

            // Limit input sensitivity for turn
            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0.2f)
                turnAmount = DetectTouchMovement.turnAngleDelta;

            if (turnAmount != 0)
                turnDirection = Math.Sign(turnAmount);

            // Zoom camera based on pinch amount
            mainCamera.orthographicSize += pinchAmount * orthoZoomSpeed;

            // Gate zoom amount
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 5f);

            // Rotate camera along Y-axis using turn angle
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                                yRotation + turnAmount,
                                                transform.eulerAngles.z);
        }

        else inputManager.isDoubleSwiping = false;
    }

    private IEnumerator AngleCorrection()
    {
        float yRotation = transform.eulerAngles.y;

        if (inputManager.isDoubleSwiping == false && yRotation % 90 != 0)
        {
            float angleDifference = yRotation % 90;

            Vector3 targetRotation = new Vector3(0, angleDifference * turnDirection, 0);

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
