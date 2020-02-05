using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    /// <summary>
    /// Marks waypoint as transitional, meaning Player will not stop at this waypoint.
    /// </summary>
    public bool isTransitional = false;

    private void Update()
    {
        if (isTransitional) GetComponentInChildren<SphereCollider>().enabled = false;
        else GetComponentInChildren<SphereCollider>().enabled = true;
    }
}
