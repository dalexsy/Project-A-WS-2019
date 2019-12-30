using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CameraRigRotation : MonoBehaviour
{
    [HideInInspector] public bool isRotating = false;
    [HideInInspector] public Vector3 averagePlankPosition;

    [SerializeField] [Range(0, 1)] private float rotationSpeed = 1f;
    [SerializeField] private AnimationCurve animationCurve = null;
    [SerializeField] private GameObject[] planks;
    [SerializeField] private Vector3 offset = new Vector3(0, -.2f, 0);

    private InputManager inputManager;
    private PauseManager pauseManager;
    private PlankManager plankManager;
    private Camera mainCamera;
    private float inputBuffer = 0;
    private float inputOffset = 0;
    private float orthoZoomSpeed = -0.003f;
    private int turnDirection = 0;
    private Vector2 startPosMouse = Vector2.zero;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        pauseManager = GameObject.Find("Game Manager").GetComponent<PauseManager>();
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
        averagePlankPosition = GetMeanVector(plankPositions);

        // Set rig's position to average Plank position
        this.transform.position = averagePlankPosition + offset;
    }

    private void LateUpdate()
    {
        if (pauseManager.isPaused) return;

        if (plankManager.hasReachedGoal)
        {
            RotateAroundLevel();
            return;
        }

        if (inputManager.isUsingTouch) TouchRotation();
        if (!inputManager.isUsingTouch && MouseInput() == 1) MouseRotation(1);
        if (!inputManager.isUsingTouch && MouseInput() == -1) MouseRotation(-1);
    }

    private void FixedUpdate()
    {
        if (!plankManager.hasReachedGoal && inputManager.isUsingTouch) AngleCorrection();
    }

    // Rotates camera around level (used after level is completed)
    private void RotateAroundLevel()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
    }

    private void TouchRotation()
    {
        if (Input.touchCount == 2)
        {
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

            if (Application.platform != RuntimePlatform.WebGLPlayer)

            {   // Zoom camera based on pinch amount
                mainCamera.orthographicSize += pinchAmount * orthoZoomSpeed;

                // Gate zoom amount
                mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 5f);
            }

            // Rotate camera along Y-axis using turn angle
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                                yRotation + turnAmount,
                                                transform.eulerAngles.z);
        }

    }

    // Resets rig to a right angle when using touch rotation
    private void AngleCorrection()
    {
        float yRotation = transform.eulerAngles.y;

        // Take remainder of current Y rotation divided by 90
        float angleDifference = yRotation % 90;

        // If angle difference is over 45, reduce angle difference to normalize correction rate
        if (angleDifference > 45)
            angleDifference = angleDifference - 90;

        // If player is not trying to rotate camera and angle difference is over half a degree
        if (Input.touchCount != 2 && Math.Abs(angleDifference) >= .5f)
        {
            // Correct rig's Y axis rotation proportionally by angle difference
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                                yRotation - angleDifference / 50,
                                                transform.eulerAngles.z);
        }
    }

    private void MouseRotation(int direction)
    {
        if (direction == 0 || isRotating) return;

        StartCoroutine(RotateRig(MouseInput()));
    }

    // Returns direction of input
    // Should be universal
    private int MouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            startPosMouse = Input.mousePosition;
            inputOffset = 0;
        }

        if (Input.GetMouseButtonUp(1))
        {
            var currentPosition = Input.mousePosition.x;
            inputOffset = currentPosition - startPosMouse.x;

            // Set input buffer to prevent input oversensitivity
            inputBuffer = Screen.width * .2f * Mathf.Sign(inputOffset);
            var direction = Mathf.Sign(inputOffset);

            // If input is over input buffer, return direction of input
            if (inputOffset > inputBuffer * direction && direction == 1) return 1;
            if (inputOffset < inputBuffer * direction && direction == -1) return -1;
        }

        // If no valid input is given, return zero
        return 0;
    }

    // Coroutine to rotate camera rig
    IEnumerator RotateRig(int direction)
    {
        // Set isRotating to true to prevent multiple rotations
        isRotating = true;

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

        // Reset mean vector
        Vector3 meanVector = Vector3.zero;

        // Add each position to mean vector
        foreach (Vector3 pos in positions)
            meanVector += pos;

        // Return mean vector divided by number of positions
        return (meanVector / positions.Count);
    }
}
