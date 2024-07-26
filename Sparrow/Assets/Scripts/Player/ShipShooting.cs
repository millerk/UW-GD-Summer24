using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject cannonBallPrefab;

    public float cannonForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!PauseMenu.GameIsPaused)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject cannonball = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = cannonball.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * cannonForce, ForceMode2D.Impulse);
    }
}
