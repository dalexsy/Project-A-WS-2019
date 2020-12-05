using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    public static PauseManager instance;
    private GameObject[] showOnPause;
    private GameObject[] hideOnPause;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        Time.timeScale = 1;
        showOnPause = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hideOnPause = GameObject.FindGameObjectsWithTag("HideOnPause");
        HidePaused();
    }

    /// <summary>
    /// Reloads current scene.
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Stops time and displays or hides pause menu.
    /// </summary>
    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            isPaused = true;
            Time.timeScale = 0;
            StartCoroutine(FadeOut());
        }

        else if (Time.timeScale == 0)
        {
            isPaused = false;
            Time.timeScale = 1;
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// Loads start screen.
    /// </summary>
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("Title Intro");
    }

    /// <summary>
    /// Shows pause screen and fades out audio
    /// </summary>
    private IEnumerator FadeOut()
    {
        ShowPaused();

        while (AudioListener.volume > 0)
            AudioListener.volume -= .01f;

        AudioListener.pause = true;
        yield return null;
    }

     /// <summary>
    /// Hides pause screen and fades in audio.
    /// </summary>
    private IEnumerator FadeIn()
    {
        HidePaused();
        AudioListener.pause = false;

        while (AudioListener.volume < 1)
            AudioListener.volume += .01f;

        yield return null;
    }

    /// <summary>
    /// Shows all UI elements tagged with ShowOnPause
    /// </summary>
    public void ShowPaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(true);

        foreach (GameObject g in hideOnPause)
            g.SetActive(false);
    }

    /// <summary>
    /// Hides all UI elements tagged with ShowOnPause
    /// </summary>
    public void HidePaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(false);

        foreach (GameObject g in hideOnPause)
            g.SetActive(true);
    }
}
