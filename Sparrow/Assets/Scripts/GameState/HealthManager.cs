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
                if (other.gameObject.GetComponent<CannonballLogic>())
                {
                    damage = other.gameObject.GetComponent<CannonballLogic>().attackStrength;
                }
                hitPoints -= damage;
                if (healthChanged != null)
                {
                    healthChanged.TriggerEvent(gameObject);
                }
                break;
            }
        }
        if (hitPoints <= 0)
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
