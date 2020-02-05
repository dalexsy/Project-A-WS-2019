using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip[] waypointSelection;
    public static PlayerAudioManager instance;
    private readonly float lowPitchRange = 1f;
    private readonly float highPitchRange = 1.2f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Plays waypoint selection sound effects.
    /// </summary>
    /// <param name="waypointPosition">Position of selected waypoint.</param>
    public void WaypointSelectionSFX(Transform waypointPosition)
    {
        UniversalAudioManager.instance.PlaySFX(waypointPosition, waypointSelection, lowPitchRange, highPitchRange);
    }
}
