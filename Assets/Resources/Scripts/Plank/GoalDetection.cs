using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        // If a plank is currently rotating, do not perform check
        if (PlankRotationManager.instance.isRotating) return;

        // If collider object has same name as this object, level is connected
        if (collider.gameObject.name.Equals(gameObject.name))
        {
            PlankManager.instance.isLevelConnected = true;
            PlankManager.instance.hasReachedGoal = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        PlankManager.instance.isLevelConnected = false;
        PlankManager.instance.hasReachedGoal = false;
    }
}
