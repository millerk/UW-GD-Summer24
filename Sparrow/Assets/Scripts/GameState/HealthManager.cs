using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public enum DamageSourceTag
    {
        PlayerAttack,
        EnemyAttack,
        Enemy
    }

    public GameEvent healthChanged;
    public GameEvent onDeath;
    public List<DamageSourceTag> damageSourceTags;
    public int HitPoints = 5;

    void OnEnable()
    {
        if (healthChanged != null)
        {
            healthChanged.TriggerEvent(gameObject);
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
                if (other.gameObject.GetComponent<CannonballLogic>())
                {
                    damage = other.gameObject.GetComponent<CannonballLogic>().attackStrength;
                }
                HitPoints -= damage;
                if (healthChanged != null)
                {
                    healthChanged.TriggerEvent(gameObject);
                }
                break;
            }
        }
        if (HitPoints <= 0)
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
        }
    }
}
