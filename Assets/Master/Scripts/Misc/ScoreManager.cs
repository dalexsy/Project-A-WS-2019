using UnityEngine.SceneManagement;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int sadnessThreshold = 10;
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

    // Sets threshold for current level
    private void SetThreshold()
    {
        string scene = SceneManager.GetActiveScene().name;
        switch (scene)
        {
            case "Level 1":
                sadnessThreshold = 10;
                perfectSolution = 2;
                break;

            case "Level 2":
                sadnessThreshold = 11;
                perfectSolution = 3;
                break;

            case "Level 3":
                sadnessThreshold = 10;
                perfectSolution = 2;
                break;

            case "Level 4":
                sadnessThreshold = 10;
                perfectSolution = 2;
                break;

            case "Level 5":
                sadnessThreshold = 20;
                perfectSolution = 6;
                break;
        }
    }
}
