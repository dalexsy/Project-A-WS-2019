using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankAudioManager : MonoBehaviour
{
    public AudioClip[] activationSuccess;
    public AudioClip[] activationFailure;

    public void ActivationSuccessSFX(Transform activePivot)
    {
        GameObject sound = new GameObject();
        sound.transform.position = activePivot.transform.position;
        int randomClip = Random.Range(0, activationSuccess.Length);
        AudioSource audioSource = sound.AddComponent<AudioSource>();
        audioSource.clip = activationSuccess[randomClip];
        audioSource.Play();
        Destroy(sound, activationSuccess[randomClip].length);
    }
}
