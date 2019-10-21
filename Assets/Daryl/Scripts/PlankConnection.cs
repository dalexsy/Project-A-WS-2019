using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankConnection : MonoBehaviour
{
    [SerializeField] Transform lPivot;
    [SerializeField] Transform rPivot;

    private Transform passivePivot = null;

    public void ConnectPlanks(Transform pivot)
    {
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
                GameObject connectedPlank = hitColliders[i].gameObject;
                CollisionDetection collisionDetection = connectedPlank.GetComponent<CollisionDetection>();

                if (collisionDetection.isCollidingWithTarget == true)
                    return;

                for (int c = 0; c < connectedPlank.transform.childCount; c++)
                {
                    Transform plankChild = connectedPlank.transform.GetChild(c);

                    if (plankChild.name.Equals(passivePivot.name))
                    {
                        //Debug.Log(plankChild.name + " is a child of " + connectedPlank);

                        plankChild.parent.parent = transform;
                    }
                }
            }
            i++;
        }
    }

    private void DisconnectPlanks()
    {

    }
}
