using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip[] waypointSelection;

    private float lowPitchRange = 1f;
    private float highPitchRange = 1.2f;

    private UniversalAudioManager universalAudioManager;

    private void Start()
    {
        universalAudioManager = GameObject.Find("SFX Manager").GetComponent<UniversalAudioManager>();
    }

    public void WaypointSelectionSFX(Transform waypointPosition)
    {
        universalAudioManager.PlaySFX(waypointPosition, waypointSelection, lowPitchRange, highPitchRange);
    }
}
