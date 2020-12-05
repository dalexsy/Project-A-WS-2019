using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    /// <summary>
    /// Reloads active scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}