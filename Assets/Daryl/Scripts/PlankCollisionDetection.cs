using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    [SerializeField] string topColliderName = "Plank Collider T";
    [SerializeField] string bottomColliderName = "Plank Collider B";

    [SerializeField] public string leftPivotName = "Pivot L";
    [SerializeField] public string rightPivotName = "Pivot R";

    private DPlankRotation plankRotation;

    private void Start()
    {
        plankRotation = GetComponentInParent<DPlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If this collider is a top collider
        if (this.gameObject.name.Equals(topColliderName))
        {
            // If this collider collides with another top collider
            if (collider.gameObject.name.Equals(topColliderName))
            {
                // If active pivot is right pivot
                if (plankRotation.pivot.name.Equals(rightPivotName))
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
        else
        {
            // If this collider collides with another bottom collider
            if (collider.gameObject.name == bottomColliderName)
            {
                // If active pivot is right pivot
                if (plankRotation.pivot.name.Equals(rightPivotName))
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

    private void OnTriggerExit(Collider collider)
    {
        // If this collider is a top collider
        if (this.gameObject.name.Equals(topColliderName))
        {
            // If this collider collides with another top collider
            if (collider.gameObject.name.Equals(topColliderName))
            {
                // If active pivot is right pivot
                if (plankRotation.pivot.name.Equals(rightPivotName))
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
        else
        {
            // If this collider collides with another bottom collider
            if (collider.gameObject.name == bottomColliderName)
            {
                // If active pivot is right pivot
                if (plankRotation.pivot.name.Equals(rightPivotName))
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
}

