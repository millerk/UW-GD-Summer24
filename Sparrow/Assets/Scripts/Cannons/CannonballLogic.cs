using UnityEngine;

public class CannonballLogic : MonoBehaviour
{

    public float maxDistance;

    public Vector3 projectileSpawnPoint;
    public float projectileSpeed;

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
}
