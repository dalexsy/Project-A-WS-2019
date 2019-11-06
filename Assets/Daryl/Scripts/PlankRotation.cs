﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankRotation : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    [SerializeField] float maxRotation = 90f;
    [SerializeField] float rotationSpeed = 1f;

    [SerializeField] GameObject pulseParticlePrefab = null;

    public Transform activePivot = null;
    public Transform surrogatePivot = null;

    public bool canRotateClockwiseR = true;
    public bool canRotateCounterclockwiseR = true;
    public bool canRotateClockwiseL = true;
    public bool canRotateCounterclockwiseL = true;
    public bool isConnectedFront = false;
    public bool isConnectedBack = false;

    private bool isRotating = false;

    private CollisionDetection collisionDetection;
    private IEnumerator connectionCoroutine;
    private PivotAssignment pivotAssignment;
    private PlankConnection plankConnection;
    private PlankManager plankManager;


    private void Start()
    {
        collisionDetection = GetComponent<CollisionDetection>();
        pivotAssignment = GetComponentInChildren<PivotAssignment>();
        plankConnection = GetComponent<PlankConnection>();

        // Defines plankManager as script in PlankManager object
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
    }

    private void Update()
    {
        // If Plank is not colliding with Player
        if (collisionDetection.isCollidingWithTarget == false)

            // Accept rotation input
            RotationInput();
    }

    private void RotationInput()
    {
        // If no pivots are given, accept no input
        if (!activePivot) return;

        // If Plank is not rotating
        if (!isRotating)
        {
            // If Plank can rotate clockwise
            if (canRotateClockwiseR && activePivot.name.Equals("Pivot R") ||
                canRotateClockwiseL && activePivot.name.Equals("Pivot L"))
            {
                if (Input.GetKeyDown("e") ||
                    Input.GetKey(KeyCode.Joystick1Button6))
                {
                    // If Plank is connected front (requires surrogate pivot)
                    if (isConnectedFront)
                    {
                        // Rotate Plank clockwise
                        // Will rotate from surrogate pivot's position
                        StartCoroutine(RotatePlank(1, activePivot));
                    }

                    else
                    {
                        // Rotate Plank clockwise 
                        // Will rotate from active pivot's position
                        StartCoroutine(RotatePlank(-1, activePivot));
                    }
                }
            }

            //  If Plank can rotate counterclockwise
            if (canRotateCounterclockwiseR && activePivot.name.Equals("Pivot R") ||
                canRotateCounterclockwiseL && activePivot.name.Equals("Pivot L"))
            {
                if (Input.GetKeyDown("q") ||
                    Input.GetKey(KeyCode.Joystick1Button7))
                {
                    // Rotate Plank counterclockwise
                    // Will rotate from surrogate pivot's position
                    if (isConnectedFront)
                    {
                        StartCoroutine(RotatePlank(-1, activePivot));
                    }

                    // Rotate Plank clockwise
                    // Will rotate from active pivot's position
                    else
                    {
                        StartCoroutine(RotatePlank(1, activePivot));
                    }
                }
            }
        }
    }

    // Rotates Plank
    // Requires direction and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned 
        Transform rotationPivot = pivot;
        Transform surrogateRotationPivot = null;
        Vector3 rotationAxis = rotationPivot.transform.right;

        // Variable used to move through animation curve
        float lerpTime = 1f;

        // Reset current lerp time
        float currentLerpTime = 0f;

        // Reset current rotation
        float currentRotation = 0f;

        // Set start rotation as Plank's x-axis rotation
        float startRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.x);

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // Start coroutine to connect planks using rotation pivot
        plankConnection.ConnectPlanks(rotationPivot);

        // If using a surrogate pivot
        if (surrogatePivot)
        {
            // Save local variable surrogateRotationPivot as surrogatePivot
            surrogateRotationPivot = surrogatePivot;

            // Assign surrogate pivot as rotationpivot
            rotationPivot = surrogateRotationPivot;

            // Redefines rotationAxis using surrogate pivot
            rotationAxis = rotationPivot.transform.right;
        }

        // Create visual feedback on pivot to be rotated from
        GameObject pulse = Instantiate(pulseParticlePrefab, rotationPivot.transform.position, pulseParticlePrefab.transform.rotation);

        // Destroy particle system once system has run once
        Destroy(pulse, pulse.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

        // While the Plank has not reached max rotation
        while (currentRotation < maxRotation)
        {
            // Increase currentLerpTime per frame
            // Rotation speed adjusts animation curve frame rate
            currentLerpTime += Time.deltaTime * rotationSpeed;

            // Gate maximum lerp time
            if (currentLerpTime > lerpTime) currentLerpTime = lerpTime;

            // Define t as percentage of lerpTime
            // Used to move through frames of animation curve
            float t = currentLerpTime / lerpTime;

            // Increase current rotation by value from animation curve
            currentRotation += animationCurve.Evaluate(t);

            // Rotate plank around given pivot in given direction
            transform.RotateAround(rotationPivot.position, rotationAxis * direction, animationCurve.Evaluate(t));

            // Returns to top of while loop
            yield return null;
        }

        // If current rotation exceeds max rotation
        if (currentRotation > maxRotation)
        {
            // Set x-axis angle to start rotation + 90 degrees
            transform.eulerAngles = new Vector3((float)roundToNearestRightAngle(transform.eulerAngles.x), transform.eulerAngles.y, transform.eulerAngles.z);
        }

        // Disconnect all connected planks
        plankConnection.DisconnectPlanks(this.transform);

        // Wait two seconds
        yield return new WaitForSeconds(2);

        // Sets isRotating to false after Plank has reached max rotation
        this.isRotating = false;
    }

    // Rounds a float to its nearest 90 degree integer
    private int roundToNearestRightAngle(float angle)
    {
        // Rounds angle to nearest int
        int roundedAngle = Mathf.FloorToInt(angle);

        // Takes remainder of rounded angle divided by 90
        int remainder = roundedAngle % 90;

        // If no remainder, return rounded angle
        if (remainder == 0) return roundedAngle;

        // If remainder is 45 or less, round angle down to nearest right angle
        if (remainder <= 45) return roundedAngle - remainder;

        // Else round angle up towards nearest right angle
        else return roundedAngle + (90 - remainder);
    }
}
