using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip[] waypointSelection;

    private float lowPitchRange = .25f;
    private float highPitchRange = .5f;

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
