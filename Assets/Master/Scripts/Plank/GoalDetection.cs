using UnityEngine;

public class GoalDetection : MonoBehaviour
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

        if (collider.gameObject.name.Equals(this.gameObject.name))
        {
            plankManager.isLevelConnected = true;
            plankManager.hasReachedGoal = true;
        }
    }
}
