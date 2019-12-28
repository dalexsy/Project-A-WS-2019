using UnityEngine;

public class NonGoalDetection : MonoBehaviour
{
    private PlankManager plankManager;
    private PlankRotationManager plankRotationManager;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (plankRotationManager.isRotating) return;
        if (collider.gameObject.name.Equals(this.gameObject.name)
            || collider.gameObject.name.Equals("Goal")) plankManager.isLevelConnected = true;
    }
}
