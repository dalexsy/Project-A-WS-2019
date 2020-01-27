using System;
using UnityEngine;

public class PivotAssignment : MonoBehaviour
{
    public bool isValid = true;
    [SerializeField] private string targetTag = null;

    private CollisionDetection collisionDetection;
    private PlankRotation plankRotation;

    private void Start()
    {
        collisionDetection = GetComponentInParent<CollisionDetection>();
        plankRotation = GetComponentInParent<PlankRotation>();
    }

    private void Update()
    {
        // If Player is currently on pivot's parent Plank and this pivot is an active pivot, start VFX
        if (PlayerManager.instance.currentPlank == transform.parent && plankRotation.activePivot == transform && isValid)
            PlankVFXManager.instance.ActivePivotVFX(transform, true);

        else
            PlankVFXManager.instance.ActivePivotVFX(transform, false);
    }

    private void OnTriggerStay(Collider collider)
    {
        // If Player enters collider range
        if (collider.gameObject.tag == targetTag)
        {
            // Assign this pivot as Plank's rotation pivot
            plankRotation.activePivot = this.transform;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // If Player leaves collider range
        if (collider.gameObject.tag == targetTag)
        {
            // Unassign this pivot as Plank's rotation pivot
            plankRotation.activePivot = null;
        }
    }
}

