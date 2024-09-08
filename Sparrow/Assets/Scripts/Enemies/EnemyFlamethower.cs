using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlamethower : MonoBehaviour
{
    public GameObject flamethrowerPrefab;
    public Transform flamethrowerSocket; // Reference to the socket

    private GameObject flamethrowerInstance;

    private void Start()
    {
        AttachFlamethrower();
    }

    private void AttachFlamethrower()
    {
        if (flamethrowerPrefab != null && flamethrowerSocket != null)
        {
            flamethrowerInstance = Instantiate(flamethrowerPrefab, flamethrowerSocket.position, Quaternion.identity, flamethrowerSocket);
        }
    }
}
