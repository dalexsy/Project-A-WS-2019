using UnityEngine.UI;
using UnityEngine;

public class GoalText : MonoBehaviour
{
    private void Update()
    {
        if (PlankManager.instance.hasReachedGoal)
        {
            var text = GetComponent<Text>();
            text.enabled = true;
        }
    }
}
