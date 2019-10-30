﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    [SerializeField] private PlankManager plankManager;

    private PlankRotation plankRotation;

    private Transform rotatingPivot;

    private void Start()
    {
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        plankRotation = GetComponentInParent<PlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(plankManager.topColliderName) ||
            this.gameObject.name.Equals(plankManager.bottomColliderName))
        {
            LimitRotationOnEnter(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(plankManager.topColliderName) ||
            this.gameObject.name.Equals(plankManager.bottomColliderName))
        {
            LimitRotationOnExit(collider);
        }
    }

    // Method to limit Plank's rotation (clockwise or counterclockwise)
    // Uses collider found on OnTriggerEnter
    private void LimitRotationOnEnter(Collider collider)
    {
        // If an active pivot is assigned
        if (plankRotation.activePivot)
        {
            // Set local variable rotatingPivot as activePivot
            rotatingPivot = plankRotation.activePivot;

            // Set rotatingPivot's name to activePivot's name
            // Ensures string checks later still work
            rotatingPivot.name = plankRotation.activePivot.name;

            this.SetRotationLimiter(false, collider);
        }
    }

    public void checkRotation(Transform pivot)
    {
        Transform[] children = pivot.parent.GetComponentsInChildren<Transform>();
        var muh = Array.FindAll(children, foundChild => foundChild.name.Equals(plankManager.topColliderName) ||
                                                        foundChild.name.Equals(plankManager.bottomColliderName));
        foreach (var bla in muh)
        {
            Debug.Log(bla.name);
            Debug.Log(bla.tag);
        }
    }

    // Method to limit Plank's rotation (clockwise or counterclockwise)
    // Uses collider found on OnTriggerExit
    private void LimitRotationOnExit(Collider collider)
    {
        // If an active pivot is assigned
        if (plankRotation.activePivot)
        {
            // Set local variable rotatingPivot as activePivot
            rotatingPivot = plankRotation.activePivot;

            // Set rotatingPivot's name to activePivot's name
            // Ensures string checks later still work
            rotatingPivot.name = plankRotation.activePivot.name;

            this.SetRotationLimiter(true, collider);
        }
    }

    private void SetRotationLimiter(bool canRotate, Collider collider)
    {
        // If this collider is a top collider that collides with another top collider
        if ((this.gameObject.name.Equals(plankManager.topColliderName)) &&
        (collider.gameObject.name.Equals(plankManager.topColliderName)))
        {
            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(plankManager.rightPivotName))
            {
                if (tag.Equals("Collider R"))
                {
                    // Pivot can rotate clockwise
                    plankRotation.canRotateClockwiseR = canRotate;
                }

                else
                {
                    plankRotation.canRotateClockwiseL = canRotate;
                }

            }
            // Else if active pivot is left pivot
            else
            {
                if (tag.Equals("Collider R"))
                {
                    // Pivot can rotate counterclockwise
                    plankRotation.canRotateCounterclockwiseR = canRotate;
                }

                else
                {
                    plankRotation.canRotateCounterclockwiseL = canRotate;
                }
            }
        }

        // If this collider is a bottom collider collides with another bottom collider
        if ((this.gameObject.name.Equals(plankManager.bottomColliderName)) &&
        (collider.gameObject.name.Equals(plankManager.bottomColliderName)))
        {
            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(plankManager.rightPivotName))
            {
                if (tag.Equals("Collider R"))
                {
                    // Pivot can rotate counterclockwise
                    plankRotation.canRotateCounterclockwiseR = canRotate;
                }

                else
                {
                    plankRotation.canRotateCounterclockwiseL = canRotate;
                }

            }
            // Else if active pivot is left pivot
            else
            {
                if (tag.Equals("Collider R"))
                {
                    // Pivot can rotate clockwise
                    plankRotation.canRotateClockwiseR = canRotate;
                }

                else
                {
                    plankRotation.canRotateClockwiseL = canRotate;
                }
            }
        }
    }
}

