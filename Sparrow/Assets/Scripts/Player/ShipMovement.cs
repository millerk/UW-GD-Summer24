using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementMode 
{
    mouseLook,
    tank
}

public class ShipMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    public MovementMode moveMode;
    public float rotateSpeed;


    Vector2 movement;
    Vector2 mousePos;
    float angle;

    void Start()
    {
        cam = Camera.main;
        if (cam == null || cam.orthographic)
        {
            Debug.LogError("Main camera is either not found or not perspective. Ensure there's a perspective camera with the 'MainCamera' tag.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (moveMode == MovementMode.mouseLook)
        {
            updateMovementMouseLook();
        } else {
            // default to tank
            updateMovementTank();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.rotation = angle - 90f;
    }

    private void updateMovementMouseLook()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    }

    private void updateMovementTank()
    {
        // Change angle by [rotateSpeed] degrees per second
        float moveDir = Input.GetAxisRaw("Vertical");
        angle -= Input.GetAxisRaw("Horizontal") * rotateSpeed * 360 * Time.deltaTime;
        // Keep within range of [-360, 360] for sanity's sake
        angle %= 360;
        
        float angleRad = angle * Mathf.Deg2Rad;
        movement.x = Mathf.Cos(angleRad) * moveDir;
        movement.y = Mathf.Sin(angleRad) * moveDir;
    }
}
