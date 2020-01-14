using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if (PlankRotationManager.instance.isRotating) return;

        if (collider.gameObject.name.Equals(this.gameObject.name))
        {
            PlankManager.instance.isLevelConnected = true;
            PlankManager.instance.hasReachedGoal = true;
        }
    }
}
