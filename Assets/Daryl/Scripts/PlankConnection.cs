using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankConnection : MonoBehaviour
{
    public void ConnectPlanks(Transform pivot)
    {
        // Look for colliders in range of this Plank's active pivot
        Collider[] hitColliders = Physics.OverlapSphere(pivot.position, 3);

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

                    if (plankChild.name.Equals(pivot.name))
                    {
                        Debug.Log(plankChild.name + " is a child of " + connectedPlank);
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
