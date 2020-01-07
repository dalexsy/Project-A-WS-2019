using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    private PlankManager plankManager;
    private PlankRotationManager plankRotationManager;
    private PlankVFXManager plankVFXManager;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        plankVFXManager = GameObject.Find("VFX Manager").GetComponent<PlankVFXManager>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (plankRotationManager.isRotating) return;

        if (collider.gameObject.name.Equals(this.gameObject.name))
        {
            plankManager.isLevelConnected = true;
            plankManager.hasReachedGoal = true;

            plankVFXManager.ActivePivotVFX(transform, true);
        }
    }
}
