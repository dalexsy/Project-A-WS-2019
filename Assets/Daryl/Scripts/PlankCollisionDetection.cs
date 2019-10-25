using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    [SerializeField] string topColliderName = "Plank Collider Top";
    [SerializeField] string bottomColliderName = "Plank Collider Bottom";
    [SerializeField] string frontColliderName = "Plank Collider Front";
    [SerializeField] string backColliderName = "Plank Collider Back";

    [SerializeField] public string leftPivotName = "Pivot L";
    [SerializeField] public string rightPivotName = "Pivot R";

    private DPlankRotation plankRotation;

    private Transform rotatingPivot;

    private void Start()
    {
        plankRotation = GetComponentInParent<DPlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Only front and back colliders detect connections with other Planks
        if (this.gameObject.name.Equals(frontColliderName) ||
            this.gameObject.name.Equals(backColliderName))
        {
            DetectConnection(collider);
        }

        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(topColliderName) ||
            this.gameObject.name.Equals(bottomColliderName))
        {
            LimitRotationOnEnter(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Only front and back colliders detect connections with other Planks
        if (this.gameObject.name.Equals(frontColliderName) ||
            this.gameObject.name.Equals(backColliderName))
        {
            DetectDisconnection(collider);
        }

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
        }

        // If this collider is a top collider
        if (this.gameObject.name.Equals(topColliderName))
        {
            // If this collider collides with another top collider
            if (collider.gameObject.name.Equals(topColliderName))
            {
                // If active pivot is right pivot
                if (rotatingPivot.name.Equals(rightPivotName))
                {
                    // Pivot cannot rotate clockwise
                    plankRotation.canRotateClockwise = false;
                }

                // Else if active pivot is left pivot
                else
                {
                    // Pivot cannot rotate counterclockwise
                    plankRotation.canRotateCounterclockwise = false;
                }
            }
        }

        // Else if this collider is a bottom collider
        if (this.gameObject.name.Equals(bottomColliderName))
        {
            // If this collider collides with another bottom collider
            if (collider.gameObject.name == bottomColliderName)
            {
                // If active pivot is right pivot
                if (rotatingPivot.name.Equals(rightPivotName))
                {
                    // Pivot cannot rotate counterclockwise
                    plankRotation.canRotateCounterclockwise = false;
                }

                // Else if active pivot is left pivot
                else
                {
                    // Pivot cannot rotate clockwise
                    plankRotation.canRotateClockwise = false;
                }
            }
        }
    }

    // Method to limit Plank's rotation (clockwise or counterclockwise)
    // Uses collider found on OnTriggerExit
    private void LimitRotationOnExit(Collider collider)
    {
        // If this collider is a top collider
        if (this.gameObject.name.Equals(topColliderName))
        {
            // If this collider collides with another top collider
            if (collider.gameObject.name.Equals(topColliderName))
            {
                // If active pivot is right pivot
                if (rotatingPivot.name.Equals(rightPivotName))
                {
                    // Pivot can rotate clockwise
                    plankRotation.canRotateClockwise = true;
                }

                // Else if active pivot is left pivot
                else
                {
                    // Pivot can rotate counterclockwise
                    plankRotation.canRotateCounterclockwise = true;
                }
            }
        }

        // If this collider is a bottom collider
        if (this.gameObject.name.Equals(bottomColliderName))
        {
            // If this collider collides with another bottom collider
            if (collider.gameObject.name == bottomColliderName)
            {
                // If active pivot is right pivot
                if (rotatingPivot.name.Equals(rightPivotName))
                {
                    // Pivot can rotate counterclockwise
                    plankRotation.canRotateCounterclockwise = true;
                }

                // Else if active pivot is left pivot
                else
                {
                    // Pivot can rotate clockwise
                    plankRotation.canRotateClockwise = true;
                }
            }
        }
    }

    // Method to detect which side a Plank is connected to (this Plank's front or back)
    // Uses collider found on OnTriggerEnter
    private void DetectConnection(Collider collider)
    {
        if (this.gameObject.name.Equals(frontColliderName) &&
        collider.tag.Equals("Plank") &&
        collider.gameObject.transform != transform.parent)
        {
            plankRotation.isConnectedFront = true;
        }

        if (this.gameObject.name.Equals(backColliderName) &&
        collider.tag.Equals("Plank") &&
        collider.gameObject.transform != transform.parent)
        {
            plankRotation.isConnectedBack = true;
        }
    }

    // Method to detect which side a Plank is disconnected from (this Plank's front or back)
    // Uses collider found on OnTriggerExit
    private void DetectDisconnection(Collider collider)
    {
        if (this.gameObject.name.Equals(frontColliderName) &&
        collider.tag.Equals("Plank") &&
        collider.gameObject.transform != transform.parent)
        {
            plankRotation.isConnectedFront = false;
        }

        if (this.gameObject.name.Equals(backColliderName) &&
        collider.tag.Equals("Plank") &&
        collider.gameObject.transform != transform.parent)
        {
            plankRotation.isConnectedBack = false;
        }
    }
}

