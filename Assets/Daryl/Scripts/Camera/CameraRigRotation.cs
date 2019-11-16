using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraRigRotation : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float rotationSpeed = 1f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] GameObject[] planks;
    List<Vector3> plankPositions = new List<Vector3>();
    private bool isRotating = false;

    private void Start()
    {
        planks = GameObject.FindGameObjectsWithTag("Plank");

    }

    private void Update()
    {
        foreach (var plank in planks)
        {
            plankPositions.Add(plank.transform.position);
        }
        var averagePosition = GetMeanVector(plankPositions);
        this.transform.position = averagePosition;
        if (isRotating) return;
        if (Input.GetKeyDown(KeyCode.Z)) StartCoroutine(RotateRig(1));
        if (Input.GetKeyDown(KeyCode.X)) StartCoroutine(RotateRig(-1));
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

            // Returns to top of while loop
            yield return null;
        }

        this.isRotating = false;
        yield return null;
    }

    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Vector3 pos in positions)
        {
            meanVector += pos;
        }

        return (meanVector / positions.Count);
    }
}
