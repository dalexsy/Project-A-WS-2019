using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Displays goal text upon level completion on screen.
/// </summary>
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
