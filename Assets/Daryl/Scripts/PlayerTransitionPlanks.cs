using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionPlanks : MonoBehaviour
{
    [SerializeField] string transitionPointName = "TransitionPoint";

    PlayerController playerController;
    PlankManager plankManager;
    PlayerPlankDetection playerPlankDetection;
    Rigidbody rigid;

    public bool isRotating = false;

    private float maxRotation = 90f;
    private float rotationSpeed = 90f;
    private float currentRotation = 0f;
    private float targetRotation = 0f;
    private float gravity = 10;

    private Vector3 playerNormal;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        playerNormal = transform.up;
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned add force downwards towards current Plank
        if (playerPlankDetection.currentPlank)
            rigid.AddForce(-gravity * rigid.mass * playerPlankDetection.currentPlank.forward);
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If no current Plank, exit method
        if (!playerPlankDetection.currentPlank) return;

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
                    plankRotation.canRotateCounterclockwiseR) return;

                // If current Plank can rotate clockwise, start rotation coroutine upwards
                if (plankRotation.canRotateClockwiseR) StartCoroutine(RotatePlayer(1));


                // If current Plank can rotate counterclockwise, start rotation coroutine downwards
                if (plankRotation.canRotateCounterclockwiseR) StartCoroutine(RotatePlayer(-1));
            }

            // If left pivot is active
            if (plankRotation.activePivot.name.Equals("Pivot L"))
            {
                // If current Plank can rotate both clockwise and counterclockwise
                // Current Plank and next Plank are parallel to one another 
                // Player does not need to rotate
                if (plankRotation.canRotateClockwiseL &&
                    plankRotation.canRotateCounterclockwiseL) return;

                // If current Plank can rotate clockwise Start rotation coroutine downwards
                if (plankRotation.canRotateClockwiseL) StartCoroutine(RotatePlayer(-1));

                // If current Plank can rotate counterclockwise
                if (plankRotation.canRotateCounterclockwiseL) StartCoroutine(RotatePlayer(1));
            }
        }
    }

    // Coroutine to rotate Player
    // Needs a direction
    IEnumerator RotatePlayer(int direction)
    {
        /*
        // Look for colliders in range of Player's position
        Collider[] firstColliders = Physics.OverlapSphere(transform.position, .1f);

        // Find point on active Plank
        // Lambda expression to find named collider
        var foundTransitionPoint = Array.Find(firstColliders, collider =>
            collider.name.Equals(plankManager.leftTransitionPointName) &&
            (collider.gameObject.transform != playerPlankDetection.currentPlank.transform));
            */

        // Reset object angle
        currentRotation = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // While the Plank has not reached max rotation
        while (currentRotation < maxRotation)
        {
            // Increase targetRotation by rotationSpeed
            // Round to integer to prevent non-integer angles from deltaTime 
            targetRotation = Mathf.RoundToInt(rotationSpeed * Time.deltaTime);

            // Increase objectAngle by targetRotation
            currentRotation += targetRotation;

            // Rotates Player forward in given direction
            transform.Rotate(targetRotation * direction, 0, 0);

            // Returns to top of while loop
            yield return null;
        }

        yield return new WaitForSeconds(1);

        this.isRotating = false;
    }
}
