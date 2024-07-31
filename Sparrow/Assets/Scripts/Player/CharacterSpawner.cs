using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterPrefab; // The character prefab to spawn

    // Method to spawn a character at the spawner's position
    public void SpawnCharacter()
    {
        if (characterPrefab != null)
        {
            Instantiate(characterPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Character prefab not assigned.");
        }
    }

}
