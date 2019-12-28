using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    private GameObject[] showOnPause;
    private GameObject[] hideOnPause;

    private void Start()
    {
        Time.timeScale = 1;
        showOnPause = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hideOnPause = GameObject.FindGameObjectsWithTag("HideOnPause");
        HidePaused();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            isPaused = true;
            Time.timeScale = 0;
            ShowPaused();
        }

        else if (Time.timeScale == 0)
        {
            isPaused = false;
            Time.timeScale = 1;
            HidePaused();
        }
    }

    public void ShowPaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(true);

        foreach (GameObject g in hideOnPause)
            g.SetActive(false);
    }

    public void HidePaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(false);

        foreach (GameObject g in hideOnPause)
            g.SetActive(true);
    }
}
