using UnityEngine;

public class PlankAudioManager : MonoBehaviour
{
    public AudioClip[] activationSuccess;
    public AudioClip[] activationFailure;
    public static PlankAudioManager instance;

    private float lowPitchRange = .75f;
    private float highPitchRange = 1.25f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void ActivationSuccessSFX(Transform activePivot)
    {
        UniversalAudioManager.instance.PlaySFX(activePivot, activationSuccess, lowPitchRange, highPitchRange);
    }

    public void ActivationFailureSFX(Transform activePivot)
    {
        UniversalAudioManager.instance.PlaySFX(activePivot, activationFailure, lowPitchRange, highPitchRange);
    }
}
