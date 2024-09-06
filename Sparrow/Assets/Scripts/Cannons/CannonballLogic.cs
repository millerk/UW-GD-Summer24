using UnityEngine;

public class CannonballLogic : MonoBehaviour
{

    public float maxDistance;
    public int attackStrength;
    public Vector3 projectileSpawnPoint;
    public float projectileSpeed;


    public GameObject explosionPrefab; // Reference to the explosion prefab
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDamage = 10f; // Damage dealt by the explosion
    public bool hasExplosives = false; // Flag to enable or disable explosive behavior


    // From inspector
    public LayerMask collisionLayerMask;
    public LayerMask damageableLayer; // Layers that can be damaged

    private Animator animator;
    
    void Start()
    {
        projectileSpawnPoint = transform.position;
        animator = GetComponent<Animator>();
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

    void OnCollisionEnter2D(Collision2D other){
        // Check if we hit something (land, enemy) that would cause us to "explode"
        // Can combine into a LayerMask check but keeping separate in case we want to
        // do something different based on the collision source
        // Check if the collision is with a layer that should trigger an explosion or destruction
        if (hasExplosives)
        {
            // If explosive, trigger explosion only if the collided object is on a damageable layer
            if ((collisionLayerMask & (1 << other.gameObject.layer)) != 0)
            {
                TriggerExplosion();
                Destroy(gameObject);
            }
        }
        else
        {
            // If not explosive, destroy the object if it collides with a layer specified in collisionLayerMask
            if ((collisionLayerMask & (1 << other.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            // Instantiate the explosion prefab at the projectile's position
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
