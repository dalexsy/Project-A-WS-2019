using System;
using UnityEngine;

public class PlankConnection : MonoBehaviour
{
    public Transform passivePivot = null;

    private Transform lPivot = null;
    private Transform rPivot = null;

    private void Start()
    {
        lPivot = transform.Find(PlankManager.instance.leftPivotName);
        rPivot = transform.Find(PlankManager.instance.rightPivotName);
    }

    // Method to connect planks using active Plank's active pivot
    public void ConnectPlanks(Transform pivot)
    {
        // If this Plank is the Player Plank
        if (PlayerManager.instance.currentPlank == transform)
        {
            // Unset any parents from Plank and exit method
            // The Player Plank never needs to be connected to other Planks
            transform.parent = null;
            return;
        }

        // Sets passive pivot as opposite of active pivot
        if (pivot.name.Equals(lPivot.name)) passivePivot = rPivot;
        else passivePivot = lPivot;

        // Look for colliders in range of this Plank's active pivot
        Collider[] hitColliders = Physics.OverlapSphere(passivePivot.position, .01f);

        // Find Plank other than this pivot's parent in firstColliders array
        // Lambda expression to find tagged collider
        var foundPivot = Array.Find(hitColliders, collider =>
        collider.tag.Equals("Pivot") &&
        collider.gameObject.transform.parent != transform);

        // If a pivot has been found
        if (foundPivot)
        {
            // If last Plank finds first Plank or first Plank finds last Plank, no need to connect them
            if ((transform == PlankManager.instance.lastPlank && foundPivot.transform.parent == PlankManager.instance.firstPlank) ||
                (transform == PlankManager.instance.firstPlank && foundPivot.transform.parent == PlankManager.instance.lastPlank)) return;

            // Set parent of found pivot as foundPlank
            Transform foundPlank = foundPivot.gameObject.transform.parent;

            // Sets this Plank as connected Plank's parent
            foundPlank.transform.parent = this.transform;

            // If found Plank is the first or last Plank, no need to connect it to anything else
            if (foundPlank == PlankManager.instance.lastPlank ||
                foundPlank == PlankManager.instance.firstPlank) return;

            // Run this method from the found Plank's script using the found pivot
            else foundPlank.GetComponent<PlankConnection>().ConnectPlanks(foundPivot.transform);
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
