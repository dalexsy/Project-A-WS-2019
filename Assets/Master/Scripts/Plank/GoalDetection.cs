using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    private PlankManager plankManager;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals(this.gameObject.name)) plankManager.hasReachedGoal = true;
    }
}
