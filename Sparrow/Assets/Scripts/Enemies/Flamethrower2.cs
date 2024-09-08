using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower2 : MonoBehaviour
{
    public float damage = 10f; // Damage per second
    public float range = 5f;   // Range of the flamethrower
    public LayerMask targetLayer; // Layer that the flamethrower can damage

    private ParticleSystem flameParticles;

    private void Start()
    {
        flameParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // Optionally, add code to handle the activation of the flamethrower.
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            // Apply damage to the target
            HealthManager targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
            {
                targetHealth.ApplyDamage(damage * Time.deltaTime, HealthManager.DamageSourceTag.EnemyAttack);
            }
        }
    }
}