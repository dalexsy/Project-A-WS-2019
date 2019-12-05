using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    public bool isTransitional = false;

    private void Update()
    {
        if (isTransitional) GetComponentInChildren<SphereCollider>().enabled = false;
        else GetComponentInChildren<SphereCollider>().enabled = true;
    }
}
