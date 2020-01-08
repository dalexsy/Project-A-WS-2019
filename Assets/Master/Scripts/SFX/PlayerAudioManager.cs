using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip[] waypointSelection;
    public static PlayerAudioManager instance;
    private float lowPitchRange = 1f;
    private float highPitchRange = 1.2f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void WaypointSelectionSFX(Transform waypointPosition)
    {
        UniversalAudioManager.instance.PlaySFX(waypointPosition, waypointSelection, lowPitchRange, highPitchRange);
    }
}
