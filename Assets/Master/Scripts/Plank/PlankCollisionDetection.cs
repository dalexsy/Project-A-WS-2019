using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    private PlankRotation plankRotation;
    private Transform rotatingPivot;
    [SerializeField] Transform collisionObject;

    private void Start()
    {
        plankRotation = GetComponentInParent<PlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(PlankManager.instance.topColliderName) ||
            this.gameObject.name.Equals(PlankManager.instance.bottomColliderName))

            LimitRotationOnEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        // Only top and bottom colliders prevent collisions with other Planks
        if (this.gameObject.name.Equals(PlankManager.instance.topColliderName) ||
            this.gameObject.name.Equals(PlankManager.instance.bottomColliderName))

            LimitRotationOnExit(collider);
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

            SetRotationLimiter(false, collider);
        }

        DetectMixedCollision(true, collider);
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

            SetRotationLimiter(true, collider);
        }
    }

    private void DetectMixedCollision(bool hasMixedCollision, Collider collider)
    {
        if ((this.gameObject.name.Equals(PlankManager.instance.topColliderName)) &&
                (collider.gameObject.name.Equals(PlankManager.instance.bottomColliderName)))
        {
            if (tag.Equals(PlankManager.instance.rightColliderTag))
            {
                Transform rightPivot = transform.parent.Find(PlankManager.instance.rightPivotName);
                rightPivot.GetComponent<PivotAssignment>().hasMixedCollisionTop = hasMixedCollision;
            }

            else if (tag.Equals(PlankManager.instance.leftColliderTag))
            {
                Transform leftPivot = transform.parent.Find(PlankManager.instance.leftPivotName);
                leftPivot.GetComponent<PivotAssignment>().hasMixedCollisionTop = hasMixedCollision;
            }
        }

        if ((this.gameObject.name.Equals(PlankManager.instance.bottomColliderName)) &&
        (collider.gameObject.name.Equals(PlankManager.instance.topColliderName)))
        {
            if (tag.Equals(PlankManager.instance.rightColliderTag))
            {
                Transform rightPivot = transform.parent.Find(PlankManager.instance.rightPivotName);
                rightPivot.GetComponent<PivotAssignment>().hasMixedCollisionBottom = hasMixedCollision;
            }

            else if (tag.Equals(PlankManager.instance.leftColliderTag))
            {
                Transform leftPivot = transform.parent.Find(PlankManager.instance.leftPivotName);
                leftPivot.GetComponent<PivotAssignment>().hasMixedCollisionBottom = hasMixedCollision;
            }
        }
    }

    private void SetRotationLimiter(bool canRotate, Collider collider)
    {
        // If this collider is a top collider that collides with another top collider
        if ((this.gameObject.name.Equals(PlankManager.instance.topColliderName)) &&
        (collider.gameObject.name.Equals(PlankManager.instance.topColliderName)))
        {
            collisionObject = collider.gameObject.transform;

            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(PlankManager.instance.rightPivotName))
            {
                // Pivot can rotate clockwise
                if (tag.Equals(PlankManager.instance.rightColliderTag)) plankRotation.canRotateClockwiseR = canRotate;
                else plankRotation.canRotateClockwiseL = canRotate;
            }

            // Else if active pivot is left pivot
            else
            {
                // Pivot can rotate counterclockwise
                if (tag.Equals(PlankManager.instance.rightColliderTag)) plankRotation.canRotateCounterclockwiseR = canRotate;
                else plankRotation.canRotateCounterclockwiseL = canRotate;
            }
        }

        // If this collider is a bottom collider collides with another bottom collider
        if ((this.gameObject.name.Equals(PlankManager.instance.bottomColliderName)) &&
        (collider.gameObject.name.Equals(PlankManager.instance.bottomColliderName)))
        {
            collisionObject = collider.gameObject.transform;

            // If active pivot is right pivot
            if (rotatingPivot.name.Equals(PlankManager.instance.rightPivotName))
            {
                // Pivot can rotate counterclockwise
                if (tag.Equals(PlankManager.instance.rightColliderTag)) plankRotation.canRotateCounterclockwiseR = canRotate;
                else plankRotation.canRotateCounterclockwiseL = canRotate;
            }

            // Else if active pivot is left pivot
            else
            {
                // Pivot can rotate clockwise
                if (tag.Equals(PlankManager.instance.rightColliderTag)) plankRotation.canRotateClockwiseR = canRotate;
                else plankRotation.canRotateClockwiseL = canRotate;
            }
        }
    }
}

