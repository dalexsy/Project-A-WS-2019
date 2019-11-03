using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionPlanks : MonoBehaviour
{
    [SerializeField] string transitionPointName = "TransitionPoint";

    DPlayerController playerController;
    PlayerPlankDetection playerPlankDetection;
    Rigidbody rigid;

    private bool isRotating = false;

    private float maxRotation = 90f;
    private float rotationSpeed = 90f;
    private float objectAngle = 0f;
    private float targetRotation = 0f;
    private float gravity = 10;

    private Vector3 myNormal;

    private void Start()
    {
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        rigid = GetComponent<Rigidbody>();
        myNormal = transform.up;
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned
        if (playerPlankDetection.currentPlank)
        {
            // Add force downwards towards current Plank
            rigid.AddForce(-gravity * rigid.mass * playerPlankDetection.currentPlank.forward);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If no current Plank, exit method
        if (!playerPlankDetection.currentPlank)
        {
            return;
        }

        // If Player collides with transition point and is not rotating
        if (collider.name.Equals(transitionPointName) && !isRotating)
        {
            // Gets plankRotation from current Plank
            // Used for its rotation limitation bools
            PlankRotation plankRotation = playerPlankDetection.currentPlank.GetComponent<PlankRotation>();

            // If right pivot is active
            if (plankRotation.activePivot.name.Equals("Pivot R"))
            {
                // If current Plank can rotate both clockwise and counterclockwise
                // Current Plank and next Plank are parallel to one another 
                // Player does not need to rotate
                if (plankRotation.canRotateClockwiseR &&
                    plankRotation.canRotateCounterclockwiseR)
                {
                    return;
                }

                // If current Plank can rotate clockwise
                if (plankRotation.canRotateClockwiseR)
                {
                    // Start rotation coroutine upwards
                    StartCoroutine(RotatePlayer(1));
                    //Debug.Log("R coroutine1");
                }

                // If current Plank can rotate counterclockwise
                if (plankRotation.canRotateCounterclockwiseR)
                {
                    // Start rotation coroutine downwards
                    StartCoroutine(RotatePlayer(-1));
                    //Debug.Log("R coroutine2");
                }
            }

            // If left pivot is active
            if (plankRotation.activePivot.name.Equals("Pivot L"))
            {
                // If current Plank can rotate both clockwise and counterclockwise
                // Current Plank and next Plank are parallel to one another 
                // Player does not need to rotate
                if (plankRotation.canRotateClockwiseL &&
                    plankRotation.canRotateCounterclockwiseL)
                {
                    return;
                }

                // If current Plank can rotate clockwise
                if (plankRotation.canRotateClockwiseL)
                {
                    // Start rotation coroutine downwards
                    StartCoroutine(RotatePlayer(-1));
                    //Debug.Log("L coroutine1");
                }

                // If current Plank can rotate counterclockwise
                if (plankRotation.canRotateCounterclockwiseL)
                {
                    // Start rotation coroutine upwards
                    StartCoroutine(RotatePlayer(1));
                    //Debug.Log("L coroutine2");
                }
            }
        }
    }

    // Coroutine to rotate Player
    // Needs a direction
    IEnumerator RotatePlayer(int direction)
    {
        // Reset object angle
        objectAngle = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        float currentAngle = transform.eulerAngles.x;

        float targetAngle = currentAngle + 90 * direction;

        // While the Plank has not reached max rotation
        while (objectAngle < maxRotation)
        {
            // Increase targetRotation by rotationSpeed
            // Round to integer to prevent non-integer angles from deltaTime 
            targetRotation = Mathf.RoundToInt(rotationSpeed * Time.deltaTime);

            // Increase objectAngle by targetRotation
            objectAngle += targetRotation;

            // Rotates Player forward in given direction
            transform.Rotate(targetRotation * direction, 0, 0);

            // Returns to top of while loop
            yield return null;
        }

        // Sets rotation cooldown to 2 seconds
        Invoke("ResetRotation", 2f);
    }

    private void ResetRotation()
    {
        // Resets rotation status
        this.isRotating = false;
    }
}
