using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public enum DamageSourceTag
    {
        PlayerAttack,
        EnemyAttack,
        Enemy,
        Explosion
    }

    public GameEvent healthChanged;
    public GameEvent onDeath;
    public List<DamageSourceTag> damageSourceTags;
    public int hitPoints = 5;
    public bool isPlayer = false;
    
    private static string PLAYER_HEALTH = "Player Health";

    void OnEnable()
    {
        if (isPlayer && GlobalVariables.Get<int>(PLAYER_HEALTH) != 0)
        {
            hitPoints = GlobalVariables.Get<int>(PLAYER_HEALTH);
        }

        if (healthChanged != null)
        {
            healthChanged.TriggerEvent(gameObject);
        }
    }

    public void OnLevelComplete(GameObject obj)
    {
        if (isPlayer)
        {
            GlobalVariables.Set(PLAYER_HEALTH, hitPoints);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        foreach (DamageSourceTag tag in damageSourceTags)
        {
            if (other.gameObject.CompareTag(tag.ToString()))
            {   
                // Default for collision damage
                int damage = 1;
                if (other.gameObject.TryGetComponent(out CannonballLogic cannonball))
                {
                    damage = cannonball.attackStrength;
                }
                ApplyDamage(damage, DamageSourceTag.PlayerAttack); // Example tag
                break;
            }
        }
        /*if (HitPoints <= 0)
        {
            if (onDeath != null)
            {
                onDeath.TriggerEvent(gameObject);
            }
            // TODO: move this to enemy specific "on death" handler since this will cause problems with player death
            // and the "if tag" branch is ehhhhhh
            if (gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }*/
    }
    public void ApplyDamage(float damageAmount, DamageSourceTag damageSourceTag)
    {
        hitPoints -= Mathf.RoundToInt(damageAmount); // Assuming damage is an integer
        if (healthChanged != null)
        {
            healthChanged.TriggerEvent(gameObject);
        }

        CheckHealthStatus();

        /*if (HitPoints <= 0)
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
        }*/
    }
    private void CheckHealthStatus()
    {
        if (hitPoints <= 0)
        {
            if (onDeath != null)
            {
                onDeath.TriggerEvent(gameObject);
            }
            if (gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }

}
