using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    [SerializeField] string topColliderName = "Plank Collider Top";
    [SerializeField] string bottomColliderName = "Plank Collider Bottom";
    [SerializeField] public string frontColliderName = "Plank Collider Front";
    [SerializeField] public string backColliderName = "Plank Collider Back";

    [SerializeField] public string leftPivotName = "Pivot L";
    [SerializeField] public string rightPivotName = "Pivot R";

    [SerializeField] public string leftColliderTag = "Collider L";
    [SerializeField] public string rightColliderTag = "Collider R";

    private PlankRotation plankRotation;

    private Transform rotatingPivot;

    private void Start()
    {
        plankRotation = GetComponentInParent<PlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(topColliderName) ||
            this.gameObject.name.Equals(bottomColliderName))
        {
            LimitRotationOnEnter(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(topColliderName) ||
            this.gameObject.name.Equals(bottomColliderName))
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
        var muh = Array.FindAll(children, foundChild => foundChild.name.Equals(topColliderName) || foundChild.name.Equals(bottomColliderName));
        foreach (var bla in muh)
        {
            Debug.Log(bla.name);
            Debug.Log(bla.tag);
        }
        //pivot.transform.();
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
        if ((this.gameObject.name.Equals(topColliderName)) && (collider.gameObject.name.Equals(topColliderName)))
        {
            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(rightPivotName))
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
        if ((this.gameObject.name.Equals(bottomColliderName)) && (collider.gameObject.name.Equals(bottomColliderName)))
        {
            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(rightPivotName))
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

