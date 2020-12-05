using UnityEngine;

public class PlankAudioManager : MonoBehaviour
{
    public AudioClip[] activationSuccess;
    public AudioClip[] activationFailure;
    public AudioClip[] goal;
    public static PlankAudioManager instance;

    private bool hasPlayedGoalSFX = false;
    private readonly float lowPitchRange = .75f;
    private readonly float highPitchRange = 1.25f;
    private Transform previousPivot;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (PlankManager.instance.hasReachedGoal && !hasPlayedGoalSFX)
        {
            hasPlayedGoalSFX = true;
            UniversalAudioManager.instance.PlaySFX(transform, goal, 1f, 1f);
        }
    }

    /// <summary>
    /// Plays rotation activation success SFX.
    /// SFX is pitched up following successive rotation from same pivot.
    /// </summary>
    /// <param name="activePivot">Pivot Player is rotating from.</param>
    public void ActivationSuccessSFX(Transform activePivot)
    {
        // If rotating from a new pivot, play new random SFX
        if (previousPivot != activePivot || !previousPivot)
        {
            UniversalAudioManager.instance.PlaySFX(activePivot, activationSuccess, lowPitchRange, highPitchRange);
        }

        // Otherwise, pitch up previous SFX and play
        else
        {
            var pitchedPitch = UniversalAudioManager.instance.previousPitch + .06f;
            var previousClip = new AudioClip[1];
            previousClip[0] = UniversalAudioManager.instance.previousClip;
            UniversalAudioManager.instance.PlaySFX(activePivot, previousClip, pitchedPitch, pitchedPitch);
        }

        previousPivot = activePivot;
    }

    /// <summary>
    /// Plays rotation activation failure SFX.
    /// </summary>
    /// <param name="activePivot">Pivot Player has tried to rotate from.</param>
    public void ActivationFailureSFX(Transform activePivot)
    {
        UniversalAudioManager.instance.PlaySFX(activePivot, activationFailure, lowPitchRange, highPitchRange);
    }
}
