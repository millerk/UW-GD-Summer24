using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonLogic : MonoBehaviour
{

    public float maxDistance;

    public Vector3 firePoint;

    void Start()
    {
        // transform.position is a vec3 because of course it is
        firePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = firePoint - transform.position;
        if (maxDistance < Mathf.Abs(distance.magnitude))
        {
            Destroy(gameObject);
        }
    }
}
