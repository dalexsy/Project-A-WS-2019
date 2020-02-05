using UnityEngine;

public class PivotOrientationDetection : MonoBehaviour
{
    [SerializeField] private Transform topCollider;
    [SerializeField] private Transform bottomCollider;
    private Vector3 topColliderPosition;
    private Vector3 bottomColliderPosition;

    private void Start()
    {
        SetColliders();
    }

    private void Update()
    {
        topColliderPosition = Camera.main.WorldToScreenPoint(topCollider.position);
        bottomColliderPosition = Camera.main.WorldToScreenPoint(bottomCollider.position);
    }

    /// <summary>
    /// Set respective bottom and top colliders of pivot.
    /// </summary>
    private void SetColliders()
    {
        for (int c = 0; c < transform.parent.childCount; c++)
        {
            var child = transform.parent.GetChild(c);

            if (gameObject.name == PlankManager.instance.leftPivotName && child.CompareTag(PlankManager.instance.leftColliderTag))
            {
                if (child.name == PlankManager.instance.topColliderName) topCollider = child.transform;
                if (child.name == PlankManager.instance.bottomColliderName) bottomCollider = child.transform;
            }

            if (gameObject.name == PlankManager.instance.rightPivotName && child.CompareTag(PlankManager.instance.rightColliderTag))
            {
                if (child.name == PlankManager.instance.topColliderName) topCollider = child.transform;
                if (child.name == PlankManager.instance.bottomColliderName) bottomCollider = child.transform;
            }
        }
    }

    /// <summary>
    /// Check if top and bottom colliders have the same X position in screenspace.
    /// </summary>
    public bool isVertical()
    {
        // If top and bottom colliders have different X positions, Plank is vertical
        if (Mathf.Round(topColliderPosition.x) != Mathf.Round(bottomColliderPosition.x)) return true;
        else return false;
    }

    /// <summary>
    /// Check if top collider is on top in screenspace
    /// Only used if Plank is horizontal.
    /// </summary>
    public bool isTopTop()
    {
        if (isVertical()) return false;
        var colliderDifference = topColliderPosition.y - bottomColliderPosition.y;
        if (colliderDifference > 0) return true;
        return false;
    }

    /// <summary>
    /// Check if top collider is on right in screenspace.
    /// Only used if Plank is horizontal.
    /// </summary>
    public bool isTopRight()
    {
        if (!isVertical()) return false;
        var colliderDifference = topColliderPosition.x - bottomColliderPosition.x;
        if (colliderDifference < 0) return true;
        return false;
    }

    /// <summary>
    /// Checks if top collider is in front of bottom.
    /// </summary>
    public bool isTopFront()
    {
        var colliderDifference = topColliderPosition.z - bottomColliderPosition.z;
        if (colliderDifference < 0) return true;
        return false;
    }
}
