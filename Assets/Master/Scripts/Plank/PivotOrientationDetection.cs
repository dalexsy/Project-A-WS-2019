using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotOrientationDetection : MonoBehaviour
{
    private PlankManager plankManager;
    [SerializeField] private Transform topCollider;
    [SerializeField] private Transform bottomCollider;
    private Vector3 topColliderPosition;
    private Vector3 bottomColliderPosition;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();

        SetColliders();
    }

    private void Update()
    {
        topColliderPosition = Camera.main.WorldToScreenPoint(topCollider.position);
        bottomColliderPosition = Camera.main.WorldToScreenPoint(bottomCollider.position);
    }

    // Set respective bottom and top colliders of pivot
    private void SetColliders()
    {
        for (int c = 0; c < transform.parent.childCount; c++)
        {
            var child = transform.parent.GetChild(c);

            if (this.gameObject.name == plankManager.leftPivotName && child.tag == plankManager.leftColliderTag)
            {
                if (child.name == plankManager.topColliderName) topCollider = child.transform;
                if (child.name == plankManager.bottomColliderName) bottomCollider = child.transform;
            }

            if (this.gameObject.name == plankManager.rightPivotName && child.tag == plankManager.rightColliderTag)
            {
                if (child.name == plankManager.topColliderName) topCollider = child.transform;
                if (child.name == plankManager.bottomColliderName) bottomCollider = child.transform;
            }
        }
    }

    // Check if top and bottom colliders have the same X position in screenspace
    public bool isVertical()
    {
        // If top and bottom colliders have different X positions, Plank is vertical
        if (topColliderPosition.x != bottomColliderPosition.x) return true;
        else return false;
    }

    // Check if top collider is on top in screenspace
    // Only used if Plank is horizontal
    public bool isTopTop()
    {
        if (isVertical()) return false;
        var colliderDifference = topColliderPosition.y - bottomColliderPosition.y;
        if (colliderDifference > 0) return true;
        return false;
    }

    // Check if top collider is on right in screenspace
    // Only used if Plank is vertical
    public bool isTopRight()
    {
        if (!isVertical()) return false;
        var colliderDifference = topColliderPosition.x - bottomColliderPosition.x;
        if (colliderDifference < 0) return true;
        return false;
    }

    // Checks if top collider is in front of bottom
    public bool isTopFront()
    {
        var colliderDifference = topColliderPosition.z - bottomColliderPosition.z;
        if (colliderDifference < 0) return true;
        return false;
    }
}
