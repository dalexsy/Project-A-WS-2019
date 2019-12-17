using UnityEngine;

public class PlankConnectionDetection : MonoBehaviour
{
    private PlankManager plankManager;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If first and last Plank are colliding, level is connected
        if (this.transform == plankManager.firstPlank && collider.gameObject.transform == plankManager.lastPlank)
            plankManager.isLevelConnected = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        // If first and last Plank are not colliding, level is not connected
        if (this.transform == plankManager.firstPlank && collider.gameObject.transform == plankManager.lastPlank)
            plankManager.isLevelConnected = false;
    }
}
