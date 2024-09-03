using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundEffect : MonoBehaviour
{
   public AudioSource audioSource;
   public AudioClip[] audioClips;

   public void PlaySound()
   {
        if (audioClips.Length > 0)
        {
            // Randomly select an audio clip from the array 
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
                audioSource.Play();
        }
   }
}
