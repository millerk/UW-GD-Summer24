using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public GameEvent enemyDeath;
    public int HitPoints = 5;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack"))
        {   
            // This won't work if we have other non-projectile attack types
            int damage = other.gameObject.GetComponent<CannonballLogic>().attackStrength;
            HitPoints -= damage;
        }
        if (HitPoints <= 0)
        {
            enemyDeath.TriggerEvent(gameObject);
            Destroy(gameObject);
        }
    }
}
