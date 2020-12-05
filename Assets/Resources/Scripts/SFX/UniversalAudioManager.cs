using UnityEngine;

public class UniversalAudioManager : MonoBehaviour
{
    public static UniversalAudioManager instance;
    public AudioClip previousClip;
    public float previousPitch;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Plays random SFX from array of audio clips at given position.
    /// </summary>
    /// <param name="sfxSource">Position SFX should originate from.</param>
    /// <param name="clips">List of clips SFX should be chosen from.</param>
    /// <param name="lowPitchRange">Lowest pitch SFX should randomly be pitched to.</param>
    /// <param name="highPitchRange">Highest pitch SFX should randomly be pitched to.</param>
    public void PlaySFX(Transform sfxSource, AudioClip[] clips, float lowPitchRange, float highPitchRange)
    {
        // Create new audio listener at SFX source
        GameObject sound = new GameObject();
        sound.transform.position = sfxSource.transform.position;
        AudioSource audioSource = sound.AddComponent<AudioSource>();

        // Select random clip from given array
        int randomClip = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomClip];
        previousClip = audioSource.clip;

        // Parent sound to source and name after parent
        sound.transform.parent = sfxSource.transform;
        sound.transform.name = sfxSource.name + " SFX";

        // Randomly pitch clip
        audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        previousPitch = audioSource.pitch;

        // Play selected clip and destroy game object after clip has ended
        audioSource.Play();
        Destroy(sound, clips[randomClip].length);
    }
}
