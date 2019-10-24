using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAssignment : MonoBehaviour
{
    [SerializeField] string targetTag = null;

    private DPlankRotation plankRotation;

    private void Start()
    {
        // Defines plankRotation as PlankRotation script from Plank
        plankRotation = GetComponentInParent<DPlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If Player enters collider range
        if (collider.gameObject.tag == targetTag)
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

            // Unassign surrogate pivot
            plankRotation.surrogatePivot = null;
        }

    }

    private void CheckForConnectedPlank(Transform activePivot)
    {

    }

    private void AssignSurrogatePivot(Transform activePivot)
    {
        // Look for colliders in range of this pivot's position
        Collider[] firstColliders = Physics.OverlapSphere(activePivot.position, .1f);

        // Lambda expression to find tagged collider
        var foundPlank = Array.Find(firstColliders, collider =>
            collider.tag.Equals("Plank") && (collider.gameObject.transform != this.transform.parent)
        );


        if (!foundPlank)
        {
            Collider[] secondColliders = Physics.OverlapSphere(activePivot.position, 1);

            var foundPivot = Array.Find(secondColliders, collider => collider.tag.Equals("Pivot") && collider.gameObject != this.gameObject);

            Transform surrogatePivot = foundPivot.gameObject.transform;

            if (foundPivot)
            {
                // Check if found pivot is child of Player Plank
                CollisionDetection collisionDetection = surrogatePivot.parent.GetComponent<CollisionDetection>();

                // If found pivot is child of Player Plank
                if (collisionDetection.isCollidingWithTarget == true)
                {
                    Debug.Log(surrogatePivot);
                    plankRotation.surrogatePivot = surrogatePivot;
                }
            }
        }

        else
        {
            return;
        }

        /* 
        for (int i = 0; i < firstColliders.Length; i++)
        {
            // If a Plank is found that is not this pivot's Plank
            if (firstColliders[i].tag.Equals("Plank") && firstColliders[i].gameObject.transform != this.transform.parent)
            {
                plankRotation.surrogatePivot = activePivot;
            }

            // Otherwise find a surrogate pivot
            else
            {
                // Look for colliders in range of this pivot
                Collider[] secondColliders = Physics.OverlapSphere(activePivot.position, 1);

                for (int p = 0; p < secondColliders.Length; p++)
                {

                    // If a pivot is found in range that is not this pivot
                    if (secondColliders[p].tag.Equals("Pivot") && secondColliders[p].gameObject != this.gameObject)
                    {

                        // Set found pivot as surrogatePivot
                        Transform foundPivot = secondColliders[p].transform;

                        // Check if found pivot is child of Player Plank
                        CollisionDetection collisionDetection = foundPivot.parent.GetComponent<CollisionDetection>();

                        //Debug.Log(collisionDetection.isCollidingWithTarget);

                        // If found pivot is child of Player Plank
                        if (collisionDetection.isCollidingWithTarget == true)
                        {
                            // Assign found pivot as surrogate of active Plank
                            plankRotation.surrogatePivot = foundPivot;
                        }
                    }
                }
            }
        }
        */
    }
}

