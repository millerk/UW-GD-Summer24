using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerParticle : MonoBehaviour
{
    public ParticleSystem flameSystem; // The particle system to activate
    public LayerMask targetLayer; // The layer(s) to detect
    public float maxDistance = 5f; // Maximum distance to activate the particle system

    private void Update()
    {
        // Check for overlapping colliders within the radius
        Collider2D hit = Physics2D.OverlapCircle(transform.position, maxDistance, targetLayer);
        if (hit != null)
        {
            Debug.Log("Overlap detected with: " + hit.gameObject.name);
            Debug.Log("Activating Particles");
            ActivateParticles();
        }
    }

    private void ActivateParticles()
    {
        if (flameSystem != null)
        {
            flameSystem.Play();
        }
    }
}