using UnityEngine;

public class NonGoalDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if (PlankRotationManager.instance.isRotating) return;
        if (collider.gameObject.name.Equals(this.gameObject.name)
            || collider.gameObject.name.Equals("Goal")) PlankManager.instance.isLevelConnected = true;
    }
}
