using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
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
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        // If level is completed, start fading out current level and load next level
        if (PlankManager.instance.hasReachedGoal)
        {
            StartCoroutine(FadeOut());
            Invoke("LoadNextLevel", timeBeforeNextLevel);
        }
    }

    // Loads next level by cycling between scenes
    private void LoadNextLevel()
    {
        string scene = SceneManager.GetActiveScene().name;
        switch (scene)
        {
            case "Level 1":
                SceneManager.LoadScene("Level 2");
                break;

            case "Level 2":
                SceneManager.LoadScene("Level 3");
                break;

            case "Level 3":
                SceneManager.LoadScene("Level 4");
                break;

            case "Level 4":
                SceneManager.LoadScene("Title Intro");
                break;
        }
    }

    // Fades transition sprite to black
    private IEnumerator FadeOut()
    {
        // Wait for given amount of time
        yield return new WaitForSeconds(timeBeforeFade);

        // Fade alpha value up to full opacity
        Color color = fade.color;
        while (fade.color.a < 1)
        {
            color.a += fadeOutRate;
            fade.color = color;
            yield return new WaitForEndOfFrame();
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
            yield return new WaitForEndOfFrame();
        }
    }
}
