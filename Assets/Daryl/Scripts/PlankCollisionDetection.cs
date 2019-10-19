using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCollisionDetection : MonoBehaviour
{
    [SerializeField] string topColliderName = "Plank Collider T";
    [SerializeField] string bottomColliderName = "Plank Collider B";

    private DPlankRotation plankRotation;
    private bool isTopCollider = false;

    private void Start()
    {
        plankRotation = GetComponentInParent<DPlankRotation>();

        if (this.gameObject.name == topColliderName)
        {
            isTopCollider = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(gameObject.name + " found " + collider.gameObject.name);

        if (isTopCollider)
        {
            if (collider.gameObject.name.Equals(topColliderName))
            {
                plankRotation.canRotateUp = false;
                Debug.Log("can't rotate up");
            }
        }

        if (!isTopCollider)
        {
            if (collider.gameObject.name == bottomColliderName)
            {
                plankRotation.canRotateDown = false;
                Debug.Log("can't rotate down");
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (isTopCollider)
        {
            if (collider.gameObject.name == topColliderName && !this.gameObject)

                plankRotation.canRotateUp = true;
        }

        if (!isTopCollider)
        {
            if (collider.gameObject.name == bottomColliderName && !this.gameObject)

                plankRotation.canRotateDown = true;
        }
    }

    /* 
    [SerializeField] string topColliderName = "Plank Collider T";
    [SerializeField] string bottomColliderName = "Plank Collider B";

    private bool isTopCollider = false;
    private DPlankRotation plankRotation;

    private void Start()
    {
        if (this.gameObject.name == topColliderName)
            isTopCollider = true;

        // Defines plankRotation as PlankRotation script from Plank
        plankRotation = GetComponentInParent<DPlankRotation>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isTopCollider)
        {
            if (collider.gameObject.name == topColliderName && !this.gameObject)
            {
                plankRotation.canRotateUp = false;
                Debug.Log("can't rotate up");
            }
        }

        if (!isTopCollider)
        {
            if (collider.gameObject.name == bottomColliderName && !this.gameObject)
            {
                plankRotation.canRotateDown = false;
                Debug.Log("can't rotate down");
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (isTopCollider)
        {
            if (collider.gameObject.name == topColliderName && !this.gameObject)

                plankRotation.canRotateUp = true;
        }

        if (!isTopCollider)
        {
            if (collider.gameObject.name == bottomColliderName && !this.gameObject)

                plankRotation.canRotateDown = true;
        }
    }

    */
}
