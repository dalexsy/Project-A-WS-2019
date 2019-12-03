using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraRigRotation : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float rotationSpeed = 1f;
    [SerializeField] private AnimationCurve animationCurve = null;
    [SerializeField] private GameObject[] planks;
    private bool isRotating = false;
    Vector2 startPos = Vector2.zero;
    Vector2 direction = Vector2.zero;

    private void Start()
    {
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

        // If rotating, accept no input
        if (isRotating) return;

        MouseRotation();
        TouchRotation();
    }

    private void TouchRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    Debug.Log(direction);
                    if (direction.x >= 25 && direction.x <= Screen.width) StartCoroutine(RotateRig(1));
                    if (direction.x <= -25 && direction.x >= -Screen.width) StartCoroutine(RotateRig(-1));
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void MouseRotation()
    {
        if (Input.GetMouseButton(0))
        {
            float moveY = Input.GetAxis("Mouse X");
            if (moveY > 0) StartCoroutine(RotateRig(1));
            if (moveY < 0) StartCoroutine(RotateRig(-1));
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
