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
            StartCoroutine(FadeOut());
        }

        else if (Time.timeScale == 0)
        {
            isPaused = false;
            Time.timeScale = 1;
            StartCoroutine(FadeIn());
        }
    }

    // Shows pause screen and fades out audio
    private IEnumerator FadeOut()
    {
        ShowPaused();

        while (AudioListener.volume > 0)
            AudioListener.volume -= .01f;

        AudioListener.pause = true;
        yield return null;
    }

    // Hides pause screen and fades in audio
    private IEnumerator FadeIn()
    {
        HidePaused();
        AudioListener.pause = false;

        while (AudioListener.volume < 1)
            AudioListener.volume += .01f;

        yield return null;
    }

    // Shows all UI elements tagged with ShowOnPause
    public void ShowPaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(true);

        foreach (GameObject g in hideOnPause)
            g.SetActive(false);
    }

    // Hides all UI elements tagged with ShowOnPause
    public void HidePaused()
    {
        foreach (GameObject g in showOnPause)
            g.SetActive(false);

        foreach (GameObject g in hideOnPause)
            g.SetActive(true);
    }
}
