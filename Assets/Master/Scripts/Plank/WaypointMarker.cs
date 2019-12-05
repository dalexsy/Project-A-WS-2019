using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    public bool isTransitional = false;
    [SerializeField] private bool isAlwaysTransitional = false;

    private void Update()
    {
        if (isAlwaysTransitional) isTransitional = true;
        if (isTransitional) GetComponentInChildren<SphereCollider>().enabled = false;
        else GetComponentInChildren<SphereCollider>().enabled = true;
    }
}
