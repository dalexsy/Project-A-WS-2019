using UnityEngine;

public class NonGoalDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        // If a plank is currently rotating, do not perform check
        if (PlankRotationManager.instance.isRotating) return;

        // If this object colliders with a goal, level is connected
        if (collider.gameObject.name.Equals(gameObject.name)
            || collider.gameObject.name.Equals("Goal")) PlankManager.instance.isLevelConnected = true;
    }
}
