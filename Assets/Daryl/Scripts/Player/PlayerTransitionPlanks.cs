﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionPlanks : MonoBehaviour
{
    [SerializeField] string transitionPointName = "Player Transition Point";
    [SerializeField] AnimationCurve animationCurve;

    public bool isRotating = false;

    private CollisionDetection collisionDetection;
    private PlayerController playerController;
    private PlankManager plankManager;
    private PlayerPlankDetection playerPlankDetection;
    private Rigidbody rigid;

    private float maxRotation = 90f;
    private float rotationSpeed = 90f;
    private float currentRotation = 0f;
    private float targetRotation = 0f;
    private float gravity = 0f;
    private int gravityDirection = -1;

    private void Start()
    {
        collisionDetection = GetComponent<CollisionDetection>();
        playerController = GetComponent<PlayerController>();
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned add force downwards towards current Plank
        if (playerPlankDetection.currentPlank)
            rigid.AddForce(-gravity * rigid.mass * playerPlankDetection.currentPlank.up * gravityDirection);
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
                if (plankRotation.canRotateClockwiseR) StartCoroutine(RotatePlayer(1 * gravityDirection));

                // If current Plank can rotate counterclockwise, start rotation coroutine downwards
                if (plankRotation.canRotateCounterclockwiseR) StartCoroutine(RotatePlayer(-1 * gravityDirection));
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
                if (plankRotation.canRotateClockwiseL) StartCoroutine(RotatePlayer(-1 * gravityDirection));

                // If current Plank can rotate counterclockwise
                if (plankRotation.canRotateCounterclockwiseL) StartCoroutine(RotatePlayer(1 * gravityDirection));
            }
        }
    }

    // Coroutine to rotate Player
    // Needs a direction
    IEnumerator RotatePlayer(int direction)
    {
        // Variable used to move through animation curve
        float lerpTime = 1f;

        // Reset current lerp time
        float currentLerpTime = 0f;

        // Reset object angle
        currentRotation = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // Vector3 to keep Player on Plank throughout rotation
        Vector3 hug = new Vector3(0, -.1f * direction, 0);

        // While the Player has not reached max rotation
        while (currentRotation < maxRotation)
        {
            // Increase currentLerpTime per frame
            // Rotation speed adjusts animation curve frame rate
            currentLerpTime += Time.deltaTime;

            // Gate maximum lerp time
            if (currentLerpTime > lerpTime) currentLerpTime = lerpTime;

            // Define t as percentage of lerpTime
            // Used to move through frames of animation curve
            float t = currentLerpTime / lerpTime;

            // Move Player forward using animation curve
            transform.Translate((Vector3.forward + hug) * animationCurve.Evaluate(t) * Time.deltaTime * .1f, Space.Self);

            // Increase targetRotation by rotationSpeed
            // Round to integer to prevent non-integer angles from deltaTime 
            targetRotation = Mathf.RoundToInt(rotationSpeed * Time.deltaTime);

            // Increase currentRotation by targetRotation
            currentRotation += targetRotation;

            float maxAngleCorrection = 0f;

            // If current rotation exceeds max rotation, set max angle correction to difference
            // Will only be used for last frame of animation
            if (currentRotation > maxRotation)
                maxAngleCorrection = currentRotation - maxRotation;

            // Rotates Player forward in given direction
            transform.Rotate((targetRotation - maxAngleCorrection) * direction, 0, 0);

            // Returns to top of while loop
            yield return null;
        }

        // Moves Player forward during cooldown period
        float cooldown = 0f;
        while (cooldown < .1f)
        {
            cooldown += Time.deltaTime;
            //transform.Translate((Vector3.forward + hug) * Time.deltaTime * playerController.moveSpeed, Space.Self);
            yield return null;
        }

        this.isRotating = false;

        yield return null;
    }
}
