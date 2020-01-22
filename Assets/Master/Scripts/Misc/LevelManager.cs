using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fade = null;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (PlankManager.instance.hasReachedGoal)
        {
            StartCoroutine(FadeOut());
            Invoke("LoadNextLevel", 4f);
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
                SceneManager.LoadScene("Level 1");
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);

        Color color = fade.color;
        while (fade.color.a < 1)
        {
            color.a += .005f;
            fade.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = fade.color;
        while (fade.color.a > 0)
        {
            color.a -= .01f;
            fade.color = color;
            yield return null;
        }
    }
}
