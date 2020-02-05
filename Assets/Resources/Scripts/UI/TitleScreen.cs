using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fade = null;
    private bool isFadingOut = false;

    // Disabled unused variable warning
#pragma warning disable 0414

    [Header("Time before fade out starts in seconds")]
    [SerializeField] [Range(0, 10)] private float timeBeforeFade = 2f;
    [SerializeField] [Range(0, 10)] private float timeBeforeNextLevel = 4f;

    [Header("Rate at which alpha value increases on fade per frame. Full opacity is 1.")]
    [SerializeField] [Range(0, .2f)] private float fadeOutRate = .005f;
    [SerializeField] [Range(0, .2f)] private float fadeInRate = .01f;

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(FadeIn());
    }

    public void StartGame()
    {
        StartCoroutine(FadeOut());
        Invoke("LoadLevel", timeBeforeNextLevel);
    }

    public void SelectLevel()
    {
        StartCoroutine(FadeOut());
        Invoke("LoadLevelSelect", timeBeforeNextLevel);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void LoadLevelSelect()
    {
        SceneManager.LoadScene("Level Selection");
    }

    // Fades transition sprite to black
    private IEnumerator FadeOut()
    {
        fade.sortingLayerName = "VFX";
        isFadingOut = true;

        // Wait for given amount of time
        yield return new WaitForSeconds(timeBeforeFade);

        // Fade alpha value up to full opacity
        Color color = fade.color;
        while (fade.color.a < 1)
        {
            color.a += fadeOutRate;
            fade.color = color;
            yield return new WaitForFixedUpdate();
        }
    }

    // Fades transition sprite to clear
    private IEnumerator FadeIn()
    {
        // Reset alpha to full opacity
        Color color = fade.color;
        color.a = 1;
        fade.color = color;

        // Fade alpha value down to zero opacity
        while (fade.color.a > 0)
        {
            if (isFadingOut) yield break;
            color.a -= fadeInRate;
            fade.color = color;
            yield return new WaitForFixedUpdate();
        }
    }
}
