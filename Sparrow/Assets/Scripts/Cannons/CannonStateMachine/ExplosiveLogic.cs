using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLogic : MonoBehaviour
{
    public float maxDistance;
    public int attackStrength;
    public Vector3 projectileSpawnPoint;
    public float projectileSpeed;
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDamage = 10f; // Damage dealt by the explosion
    public LayerMask damageableLayer; // Layer(s) that can be damaged

    // From inspector
    private int _enemyLayer = 8;
    private int _grassLayer = 10;

    void Start()
    {
        projectileSpawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _distance = projectileSpawnPoint - transform.position;
        if (maxDistance < Mathf.Abs(_distance.magnitude))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if we hit something (land, enemy) that would cause us to "explode"
        // Can combine into a LayerMask check but keeping separate in case we want to
        // do something different based on the collision source
        if (other.gameObject.layer == _enemyLayer ||
            other.gameObject.layer == _grassLayer)
        {

            TriggerExplosion();
            Destroy(gameObject);
        }
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
}
