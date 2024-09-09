using UnityEngine;

public class PlaySelectedSoundEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    // Method to play a specific sound based on the index in the array
    public void PlaySound(int index)
    {
        if (audioClips.Length > 0 && index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Invalid sound index or audio clips not set.");
        }
    }
}