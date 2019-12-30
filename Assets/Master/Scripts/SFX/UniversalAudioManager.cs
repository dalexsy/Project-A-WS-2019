using UnityEngine;

public class UniversalAudioManager : MonoBehaviour
{
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

        // Randomly pitch clip
        audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);

        // Play selected clip and destroy game object after clip has ended
        audioSource.Play();
        Destroy(sound, clips[randomClip].length);
    }
}
