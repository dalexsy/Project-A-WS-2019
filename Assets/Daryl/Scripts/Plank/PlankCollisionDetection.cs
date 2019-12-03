using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    private PlankManager plankManager;
    private PlankRotation plankRotation;
    private Transform rotatingPivot;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
        plankRotation = GetComponentInParent<PlankRotation>();
    }

    private void OnTriggerStay(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(plankManager.topColliderName) ||
            this.gameObject.name.Equals(plankManager.bottomColliderName))
            LimitRotationOnStay(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(plankManager.topColliderName) ||
            this.gameObject.name.Equals(plankManager.bottomColliderName))
            LimitRotationOnExit(collider);
    }

    // Method to limit Plank's rotation (clockwise or counterclockwise)
    // Uses collider found on OnTriggerStay
    private void LimitRotationOnStay(Collider collider)
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
                // Pivot can rotate clockwise
                if (tag.Equals(plankManager.rightColliderTag)) plankRotation.canRotateClockwiseR = canRotate;
                else plankRotation.canRotateClockwiseL = canRotate;
            }

            // Else if active pivot is left pivot
            else
            {
                // Pivot can rotate counterclockwise
                if (tag.Equals(plankManager.rightColliderTag)) plankRotation.canRotateCounterclockwiseR = canRotate;
                else plankRotation.canRotateCounterclockwiseL = canRotate;
            }
        }

        // If this collider is a bottom collider collides with another bottom collider
        if ((this.gameObject.name.Equals(plankManager.bottomColliderName)) &&
        (collider.gameObject.name.Equals(plankManager.bottomColliderName)))
        {
            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(plankManager.rightPivotName))
            {
                // Pivot can rotate counterclockwise
                if (tag.Equals(plankManager.rightColliderTag)) plankRotation.canRotateCounterclockwiseR = canRotate;
                else plankRotation.canRotateCounterclockwiseL = canRotate;
            }

            // Else if active pivot is left pivot
            else
            {
                // Pivot can rotate clockwise
                if (tag.Equals(plankManager.rightColliderTag)) plankRotation.canRotateClockwiseR = canRotate;
                else plankRotation.canRotateClockwiseL = canRotate;
            }
        }
    }
}

