﻿using UnityEngine;

public class PlankAudioManager : MonoBehaviour
{
    public AudioClip[] activationSuccess;
    public AudioClip[] activationFailure;
    public static PlankAudioManager instance;

    private float lowPitchRange = .75f;
    private float highPitchRange = 1.25f;
    private Transform previousPivot;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

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

    public void ActivationFailureSFX(Transform activePivot)
    {
        UniversalAudioManager.instance.PlaySFX(activePivot, activationFailure, lowPitchRange, highPitchRange);
    }
}
