using UnityEngine.UI;
using UnityEngine;

public class GoalText : MonoBehaviour
{
    private PlankManager plankManager;

    private void Start()
    {
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
    }

    private void Update()
    {
        if (plankManager.hasReachedGoal)
        {
            var text = GetComponent<Text>();
            text.enabled = true;
        }
    }
}
