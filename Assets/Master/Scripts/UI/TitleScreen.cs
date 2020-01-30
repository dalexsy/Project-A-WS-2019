using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fade = null;

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
        ResetAlpha();
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
        SceneManager.LoadScene("Level 1");
    }

    // Sets alpha value of sprite renderer back to full opacity
    private void ResetAlpha()
    {
        Color color = fade.color;
        color.a = 1;
        fade.color = color;
    }

    // Fades transition sprite to black
    private IEnumerator FadeOut()
    {
        fade.sortingLayerName = "VFX";

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
        // Fade alpha value down to zero opacity
        Color color = fade.color;
        while (fade.color.a > 0)
        {
            color.a -= fadeInRate;
            fade.color = color;
            yield return new WaitForFixedUpdate();
        }
    }
}
