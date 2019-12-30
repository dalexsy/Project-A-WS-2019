using UnityEngine;

public class PlankAudioManager : MonoBehaviour
{
    public AudioClip[] activationSuccess;
    public AudioClip[] activationFailure;

    private float lowPitchRange = .75f;
    private float highPitchRange = 1.25f;

    private UniversalAudioManager universalAudioManager;

    private void Start()
    {
        universalAudioManager = GameObject.Find("SFX Manager").GetComponent<UniversalAudioManager>();
    }

    public void ActivationSuccessSFX(Transform activePivot)
    {
        universalAudioManager.PlaySFX(activePivot, activationSuccess, lowPitchRange, highPitchRange);
    }

    public void ActivationFailureSFX(Transform activePivot)
    {
        universalAudioManager.PlaySFX(activePivot, activationFailure, lowPitchRange, highPitchRange);
    }
}
