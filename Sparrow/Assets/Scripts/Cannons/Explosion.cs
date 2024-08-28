using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 5f; // Radius of the explosion
    public float damage = 10f; // Damage dealt by the explosion
    public LayerMask damageableLayer; // Layer(s) that can be damaged

    private void Start()
    {
        // Optionally, add a delay before exploding or immediately trigger explosion
        Invoke("Explode", 0.2f); // Adjust the delay as needed
    }

    private void Explode()
    {
        // Get all colliders in the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageableLayer);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has a HealthManager component
            HealthManager healthManager = collider.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                // Apply damage
                healthManager.ApplyDamage(damage, HealthManager.DamageSourceTag.Explosion);
            }
        }

        // Optional: Play explosion animation or particle effect
        // e.g., GetComponent<ParticleSystem>().Play();

        // Destroy the explosion GameObject after it has done its job
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red sphere in the scene view to visualize the explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

// add to healthmanager
/*public void ApplyDamage(float damageAmount)
{
    HitPoints -= Mathf.RoundToInt(damageAmount); // Assuming damage is an integer
    if (healthChanged != null)
    {
        healthChanged.TriggerEvent(gameObject);
    }
    if (HitPoints <= 0)
    {
        if (onDeath != null)
        {
            onDeath.TriggerEvent(gameObject);
        }
        // Destroy the GameObject if it's an enemy (specific handling can be done here)
        if (gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}*/


//apply to projectile
/*
public class Projectile : MonoBehaviour
{
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDamage = 10f; // Damage dealt by the explosion
    public LayerMask damageableLayer; // Layer(s) that can be damaged

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile collides with something
        // Optionally add conditions for specific targets if needed
        TriggerExplosion();
        
        // Destroy the projectile after it triggers the explosion
        Destroy(gameObject);
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            // Instantiate the explosion prefab at the projectile's position and rotation
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Set the explosion parameters
            Explosion explosionScript = explosion.GetComponent<Explosion>();
            if (explosionScript != null)
            {
                explosionScript.radius = explosionRadius;
                explosionScript.damage = explosionDamage;
                explosionScript.damageableLayer = damageableLayer;
            }
        }
    }
}*/