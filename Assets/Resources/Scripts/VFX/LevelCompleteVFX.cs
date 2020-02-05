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

    /// <summary>
    /// Plays VFX on level completion.
    /// Starts particle system on all systems tagged with Level Complete.
    /// </summary>
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
