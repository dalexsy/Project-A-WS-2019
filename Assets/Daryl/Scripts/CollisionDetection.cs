using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] string targetTag = null;

    private bool isCollidingWithTarget = false;

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
}
