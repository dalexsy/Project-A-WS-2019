using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] string targetTag = null;
    [SerializeField] bool isTrigger = false;

    public bool isCollidingWithTarget = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetTag)
            this.isCollidingWithTarget = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == targetTag)
            this.isCollidingWithTarget = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isTrigger)
            return;

        if (collider.gameObject.tag == targetTag)
            this.isCollidingWithTarget = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!isTrigger)
            return;

        if (collider.gameObject.tag == targetTag)
            this.isCollidingWithTarget = false;
    }
}
