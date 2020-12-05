using UnityEngine.SceneManagement;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    /// <summary>
    /// The number of moves before the Player becomes sad.
    /// </summary>
    public int sadnessThreshold = 10;

    /// <summary>
    /// The number of moves required for a perfect solution.
    /// </summary>
    public int perfectSolution = 2;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        SetThreshold();
    }


    /// <summary>
    /// Sets threshold for current level.
    /// </summary>
    private void SetThreshold()
    {
        string scene = SceneManager.GetActiveScene().name;
        switch (scene)
        {
            case "Level 1":
                sadnessThreshold = 10;
                perfectSolution = 2;
                PlayerAnimationManager.instance.shouldCelebrate = true;
                break;

            case "Level 2":
                sadnessThreshold = 11;
                perfectSolution = 3;
                PlayerAnimationManager.instance.shouldCelebrate = true;
                break;

            case "Level 3":
                sadnessThreshold = 10;
                perfectSolution = 2;
                PlayerAnimationManager.instance.shouldCelebrate = true;
                break;

            case "Level 4":
                sadnessThreshold = 10;
                perfectSolution = 2;
                PlayerAnimationManager.instance.shouldCelebrate = true;
                break;

            case "Level 5":
                sadnessThreshold = 20;
                perfectSolution = 6;
                PlayerAnimationManager.instance.shouldCelebrate = false;
                break;
        }
    }
}
