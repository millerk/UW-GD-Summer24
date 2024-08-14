using UnityEngine;

public class CannonballLogic : MonoBehaviour
{

    public float maxDistance;
    public int attackStrength;
    public Vector3 projectileSpawnPoint;
    public float projectileSpeed;

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

    void OnCollisionEnter2D(Collision2D other){
        // Check if we hit something (land, enemy) that would cause us to "explode"
        // Can combine into a LayerMask check but keeping separate in case we want to
        // do something different based on the collision source
        if (other.gameObject.layer == _enemyLayer ||
            other.gameObject.layer == _grassLayer)
        {
            Destroy(gameObject);
        }
    }
}
