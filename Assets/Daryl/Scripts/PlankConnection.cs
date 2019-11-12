using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankConnection : MonoBehaviour
{
    [SerializeField] Transform lPivot = null;
    [SerializeField] Transform rPivot = null;

    private Transform passivePivot = null;

    private CollisionDetection collisionDetection;
    private PlankManager plankManager;

    private void Start()
    {
        collisionDetection = GetComponent<CollisionDetection>();
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();

        lPivot = transform.Find(plankManager.leftPivotName);
        rPivot = transform.Find(plankManager.rightPivotName);
    }

    // Method to connect planks using active Plank's active pivot
    public void ConnectPlanks(Transform pivot)
    {
        // If this Plank is the Player Plank
        if (collisionDetection.isCollidingWithTarget)
        {
            // Unset any parents from Plank and exit method
            // The Player Plank never needs to be connected to other Planks
            this.transform.parent = null;
            return;
        }

        // Sets passive pivot as opposite of active pivot
        if (pivot.name.Equals(lPivot.name))
        {
            passivePivot = rPivot;
        }

        else
        {
            passivePivot = lPivot;
        }

        // Look for colliders in range of this Plank's active pivot
        Collider[] hitColliders = Physics.OverlapSphere(this.passivePivot.position, .01f);

        // Find Plank other than this pivot's parent in firstColliders array
        // Lambda expression to find tagged collider
        var foundPivot = Array.Find(hitColliders, collider =>
        collider.tag.Equals("Pivot") &&
        collider.gameObject.transform.parent != this.transform);

        // If a pivot has been found
        if (foundPivot)
        {
            // If last Plank finds first Plank or first Plank finds last Plank
            if ((this.transform == plankManager.lastPlank && foundPivot.transform.parent == plankManager.firstPlank) ||
                (this.transform == plankManager.firstPlank && foundPivot.transform.parent == plankManager.lastPlank))
            {
                // No need to connect them
                return;
            }

            // Set parent of found pivot as foundPlank
            Transform foundPlank = foundPivot.gameObject.transform.parent;

            // Sets this Plank as connected Plank's parent
            foundPlank.transform.parent = this.transform;

            // If found Plank is the first or last Plank
            if (foundPlank == plankManager.lastPlank ||
                foundPlank == plankManager.firstPlank)
            {
                // No need to connect it to anything else
                return;
            }

            else
            {
                // Run this method from the found Plank's script using the found pivot
                foundPlank.GetComponent<PlankConnection>().ConnectPlanks(foundPivot.transform);
            }
        }
    }

    // Method to disconnect Planks using the active Plank's transform
    public void DisconnectPlanks(Transform activePlank)
    {
        // Goes through active Plank's children
        for (int c = 0; c < activePlank.transform.childCount; c++)
        {
            // Defines connectedPlank as a found child of active Plank
            Transform connectedPlank = activePlank.transform.GetChild(c);

            // If a child is found tagged with Plank
            if (connectedPlank.tag.Equals("Plank"))
            {
                // Run this script in connected Plank
                connectedPlank.GetComponent<PlankConnection>().DisconnectPlanks(connectedPlank);

                // Unset connected plank's parent
                connectedPlank.parent = null;
            }
        }
    }
}
