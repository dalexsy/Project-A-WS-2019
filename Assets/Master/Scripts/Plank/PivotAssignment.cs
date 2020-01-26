﻿using System;
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
        if (collider.gameObject.tag == targetTag && isValid)
        {
            // Assign this pivot as Plank's rotation pivot
            plankRotation.activePivot = this.transform;
            AssignSurrogatePivot(transform);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // If Player leaves collider range
        if (collider.gameObject.tag == targetTag)
        {
            // Unassign this pivot as Plank's rotation pivot
            plankRotation.activePivot = null;

            // Unassign surrogate pivot from Plank
            plankRotation.surrogatePivot = null;

            // Resets isConnectedFront in Plank
            // Only used with surrogate pivot
            plankRotation.isConnectedFront = false;

            // Resets isConnectedBack in Plank
            // Only used with surrogate pivot
            plankRotation.isConnectedBack = false;
        }
    }

    private void AssignSurrogatePivot(Transform activePivot)
    {
        // Look for colliders in range of this pivot's position
        Collider[] firstColliders = Physics.OverlapSphere(activePivot.position, .1f);

        // Find Plank other than this pivot's parent in firstColliders array
        // Lambda expression to find tagged collider
        var foundPlank = Array.Find(firstColliders, collider =>
            collider.tag.Equals("Plank") &&
            collider.gameObject.transform != this.transform.parent
        );

        // If Plank is found, active pivot can be used for rotation
        if (foundPlank)
        {
            plankRotation.activePivot = this.transform;
            return;
        }

        // If no Plank is found
        else
        {
            // Look for colliders in range of this pivot's position
            Collider[] secondColliders = Physics.OverlapSphere(activePivot.position, .1f);

            // Find pivot other than this pivot in secondColliders array
            var foundPivot = Array.Find(secondColliders, collider =>
            collider.tag.Equals("Pivot") &&
            collider.gameObject != this.gameObject);

            // If a pivot has been found
            if (foundPivot)
            {
                // Set local variable surrogatePivot to foundPivot's transform
                Transform surrogatePivot = foundPivot.gameObject.transform;

                // Set plank's surrogatePivot to local surrogatePivot
                plankRotation.surrogatePivot = surrogatePivot;

                // Look for colliders in range of surrogate pivot's position
                Collider[] thirdColliders = Physics.OverlapSphere(surrogatePivot.position, .5f);

                // Find front in thirdColliders array
                var foundFrontCollider = Array.Find(secondColliders, collider =>
                collider.name.Equals(PlankManager.instance.frontColliderName) &&
                collider.gameObject == this.gameObject);

                // If a front collider is found Plank is connected front
                if (foundFrontCollider) plankRotation.isConnectedFront = true;

                // Else Plank is connected back
                else plankRotation.isConnectedBack = true;
            }
        }
    }
}

