using UnityEngine;

public class LevelCompleteVFX : MonoBehaviour
{
    private GameObject[] levelCompletePrefabs;
    private bool hasTriggered = false;

    private void Start()
    {
        levelCompletePrefabs = GameObject.FindGameObjectsWithTag("Level Complete");
    }

    private void Update()
    {
        if (!hasTriggered) PlayLevelCompleteVFX();
    }

    private void PlayLevelCompleteVFX()
    {
        if (PlankManager.instance.hasReachedGoal)
        {
            foreach (GameObject g in levelCompletePrefabs)
            {
                var pS = g.GetComponent<ParticleSystem>();
                pS.Play();
                hasTriggered = true;
            }
        }
    }
}
