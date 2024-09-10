using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    public int maxHealth;
    public int hitPoints = 5;
    public bool isPlayer = false;
    public GameObject model;
    public float additionalDashIFrame;
    
    [SerializeField]
    private float hitInvulnDurationSeconds;
    [SerializeField]
    private float invulnDeltaTime;

    private bool isInvuln = false;
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
        if (isInvuln) return;

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
                StartCoroutine(BecomeInvuln());
                break;
            }
        }
    }
    public void ApplyDamage(float damageAmount, DamageSourceTag damageSourceTag)
    {
        if (isInvuln) return; // Handle case where explosion applies direct damage

        hitPoints -= Mathf.RoundToInt(damageAmount); // Assuming damage is an integer
        if (healthChanged != null)
        {
            healthChanged.TriggerEvent(gameObject);
        }

        CheckHealthStatus();
    }

    public void ApplyHealing(GameObject healingSource)
    {
        hitPoints += healingSource.GetComponent<SoulHealth>().value;
        hitPoints = Math.Min(hitPoints, maxHealth);
        if (healthChanged != null)
        {
            healthChanged.TriggerEvent(gameObject);
        }
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

    private IEnumerator BecomeInvuln()
    {
        isInvuln = true;
        for (float i = 0; i < hitInvulnDurationSeconds; i += invulnDeltaTime)
        {
            if (model.transform.localScale == Vector3.one)
            {
                ScaleModelTo(Vector3.zero);
            }
            else
            {
                ScaleModelTo(Vector3.one);
            }
            yield return new WaitForSeconds(invulnDeltaTime);

        }
        ScaleModelTo(Vector3.one);
        isInvuln = false;
    }

    private void ScaleModelTo(Vector3 scale)
    {
        model.transform.localScale = scale;
    }
    public void PlayerDashed(GameObject player)
    {
        float invulntime = player.GetComponent<ShipMovement>().dashDuration + additionalDashIFrame;
        StartCoroutine(ApplyDashIFrames(invulntime));
    }       

    private IEnumerator ApplyDashIFrames(float dashDuration)
    {
        isInvuln = true;
        for (float i = 0; i < dashDuration; i += invulnDeltaTime)
        {
            yield return new WaitForSeconds(invulnDeltaTime);

        }
        isInvuln = false;
    }

}
