using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlankConnection : MonoBehaviour
{
    [SerializeField] Transform lPivot;
    [SerializeField] Transform rPivot;

    private Transform passivePivot = null;

    // Coroutine to connect planks using active Plank's active pivot
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
                    Transform connectedPlankPivot = connectedPlank.transform.GetChild(c);

                    if (connectedPlankPivot.name.Equals(passivePivot.name))
                    {
                        ConnectPlanks(connectedPlankPivot);
                        //Debug.Log(connectedPlank + " is connecting " + plankChild.parent);
                    }
                }
            }
        }
    }

    // Coroutine to disconnect planks using the active Plank's transform
    public IEnumerator DisconnectPlanks(Transform activePlank)
    {
        // Goes through active Plank's children
        for (int c = 0; c < activePlank.transform.childCount; c++)
        {
            // Defines connectedPlank as a found child of active Plank
            Transform connectedPlank = activePlank.transform.GetChild(c);

            // If a child is found tagged with Plank
            if (connectedPlank.tag.Equals("Plank"))
            {
                // Unset connected plank's parent
                connectedPlank.parent = null;
            }
        }

        // Exits coroutine upon completion
        yield break;
    }

}
