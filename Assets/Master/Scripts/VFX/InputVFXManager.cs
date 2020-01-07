using UnityEngine;

public class InputVFXManager : MonoBehaviour
{
    public GameObject waypointSelectPrefab;

    private UniversalVFXManager universalVFXManager;

    private void Start()
    {
        universalVFXManager = GameObject.Find("VFX Manager").GetComponent<UniversalVFXManager>();
    }

    public void WaypointSelectionVFX(Transform targetWaypoint)
    {
        universalVFXManager.PlayVFX(targetWaypoint, waypointSelectPrefab, new Vector3(0, .06f, 0));
    }
}
