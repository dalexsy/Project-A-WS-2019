using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
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
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (PlankManager.instance.hasReachedGoal)
        {
            StartCoroutine(FadeOut());
            Invoke("LoadNextLevel", timeBeforeNextLevel);
        }
    }

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
                SceneManager.LoadScene("Level 1");
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeBeforeFade);

        Color color = fade.color;
        while (fade.color.a < 1)
        {
            color.a += fadeOutRate;
            fade.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = fade.color;
        while (fade.color.a > 0)
        {
            color.a -= fadeInRate;
            fade.color = color;
            yield return null;
        }
    }
}
