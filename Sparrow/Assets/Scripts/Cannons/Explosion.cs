using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 0.5f; // Radius of the explosion
    public float damage = 10f; // Damage dealt by the explosion
    public LayerMask damageableLayer; // Layer(s) that can be damaged
    public float animationDuration = 1f; // Duration of the explosion animation
    public Vector3 initialScale = Vector3.one; // Initial scale
    public Vector3 finalScale = Vector3.one * 2f; // Final scale

    private Animator animator;
    private bool isExploding = false;

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Optionally, add a delay before exploding or immediately trigger explosion
        Invoke("Explode", 0.2f); // Adjust the delay as needed

        // Set the initial scale
        transform.localScale = initialScale;
    }

    private void Explode()
    {
        if (isExploding)
            return;

        isExploding = true;

        // Play the explosion animation
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        // Debug log to check radius value
        Debug.Log("Explosion radius: " + radius);

        // Get all colliders within a large radius
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, radius * 10f); // Large radius to ensure coverage

        foreach (Collider2D collider in allColliders)
        {
            if ((damageableLayer & (1 << collider.gameObject.layer)) != 0)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance <= radius)
                {
                    // Debug log to check detected collider positions
                    Debug.Log("Detected collider at position: " + collider.transform.position);

                    HealthManager healthManager = collider.GetComponent<HealthManager>();
                    if (healthManager != null)
                    {
                        healthManager.ApplyDamage(damage, HealthManager.DamageSourceTag.Explosion);
                    }
                }
            }
        }

        // Start the coroutine to handle scaling and destruction
        StartCoroutine(HandleExplosion());
    }

    private IEnumerator HandleExplosion()
    {
        // Scale up the explosion
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;

        // Optionally, visualize the explosion radius
        Debug.Log("Explosion radius: " + radius);

        // Destroy the GameObject
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red sphere in the scene view to visualize the explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}