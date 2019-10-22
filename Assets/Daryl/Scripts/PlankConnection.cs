using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankConnection : MonoBehaviour
{
    [SerializeField] Transform lPivot;
    [SerializeField] Transform rPivot;

    private Transform passivePivot = null;

    public IEnumerator ConnectPlanks(Transform pivot)
    {
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
        Collider[] hitColliders = Physics.OverlapSphere(passivePivot.position, 1);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            // If a Plank is found that is not this Plank
            if (hitColliders[i].tag.Equals("Plank") && hitColliders[i].gameObject != this.gameObject)
            {
                // Sets found game object as connectedPlank
                GameObject connectedPlank = hitColliders[i].gameObject;

                // Checks if connected plank is the Player Plank
                CollisionDetection collisionDetection = connectedPlank.GetComponent<CollisionDetection>();

                // If so, exit method
                if (collisionDetection.isCollidingWithTarget == true)
                    yield break;

                // Sets this plank as connected plank's parent
                connectedPlank.transform.parent = transform;

                // Searches for pivots in connected plank
                for (int c = 0; c < connectedPlank.transform.childCount; c++)
                {
                    Transform plankChild = connectedPlank.transform.GetChild(c);

                    if (plankChild.name.Equals(passivePivot.name))
                    {
                        ConnectPlanks(plankChild);
                        //Debug.Log(connectedPlank + " is connecting " + plankChild.parent);
                    }
                }
            }
        }
    }

    /* 
        // Method to connect Planks to active Plank
        // Uses pivot from PivotAssignment
        public void ConnectPlanks(Transform pivot)
        {
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
            Collider[] hitColliders = Physics.OverlapSphere(passivePivot.position, 1);

            int i = 0;
            while (i < hitColliders.Length)
            {
                // If a Plank is found that is not this Plank
                if (hitColliders[i].tag.Equals("Plank") && hitColliders[i].gameObject != this.gameObject)
                {
                    // Sets found game object as connectedPlank
                    GameObject connectedPlank = hitColliders[i].gameObject;

                    // Checks if connected plank is the Player Plank
                    CollisionDetection collisionDetection = connectedPlank.GetComponent<CollisionDetection>();

                    // If so, exit method
                    if (collisionDetection.isCollidingWithTarget == true)
                        return;

                    // Sets this plank as connected plank's parent
                    connectedPlank.transform.parent = transform;

                    // Searches for pivots in connected plank
                    for (int c = 0; c < connectedPlank.transform.childCount; c++)
                    {
                        Transform plankChild = connectedPlank.transform.GetChild(c);

                        if (plankChild.name.Equals(passivePivot.name))
                        {
                            ConnectPlanks(plankChild);
                        }
                    }
                }

                i++;
            }
        }
        */

    private void DisconnectPlanks()
    {

    }
}
