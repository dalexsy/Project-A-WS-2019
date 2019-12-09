using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraRigRotation : MonoBehaviour
{
    [SerializeField] private GameObject[] planks;
    private InputManager inputManager;
    private Camera mainCamera;
    private float orthoZoomSpeed = 0.01f;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

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

    void LateUpdate()
    {
        float pinchAmount = 0;
        //Quaternion desiredRotation = transform.rotation;

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
            //desiredRotation *= Quaternion.Euler(rotationDeg);
        }

        // ... change the orthographic size based on the change in distance between the touches.
        mainCamera.orthographicSize += pinchAmount * orthoZoomSpeed;

        // Make sure the orthographic size never drops below zero.
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 5f);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation + pinchAmount, transform.eulerAngles.z);
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
