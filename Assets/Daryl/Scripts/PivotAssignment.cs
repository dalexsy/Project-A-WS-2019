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

            // Assign this pivot as Plank's rotation pivot
            plankRotation.activePivot = this.transform;
    }



    private void OnTriggerExit(Collider collider)
    {
        // If Player leaves collider range
        if (collider.gameObject.tag == targetTag)

            // Unassign this pivot as Plank's rotation pivot
            plankRotation.activePivot = null;
    }
}