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

    // Plays random SFX from array of audio clips at given position
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

        // Randomly pitch clip
        audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        previousPitch = audioSource.pitch;

        // Play selected clip and destroy game object after clip has ended
        audioSource.Play();
        Destroy(sound, clips[randomClip].length);
    }
}
