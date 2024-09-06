using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MovementMode
{
    mouseLook,
    tank
}

public class ShipMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f; // Speed during dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 1f; // Cooldown between dashes
    public float dashCooldownRemaining = 0f;
    public GameEvent dashCooldownUpdated;
    public GameEvent playerDashed;

    public Rigidbody2D rb;
    public Camera cam;
    public MovementMode moveMode;
    public float rotateSpeed;
    
    private Vector2 movement;
    private Vector2 mousePos;
    private float angle;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;

    public AudioClip dashSound; // Reference to the dash sound effect
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        cam = Camera.main;
        if (cam == null || cam.orthographic)
        {
            Debug.LogError("Main camera is either not found or not perspective. Ensure there's a perspective camera with the 'MainCamera' tag.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the GameObject. Please add one.");
        }
    }
    void Update()
    {
        if (moveMode == MovementMode.mouseLook)
        {
            updateMovementMouseLook();
        }
        else
        {
            // default to tank
            updateMovementTank();
        }

        // Dash functionality
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
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

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Play the dash sound effect
        if (audioSource != null && dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }

        float angleRad = angle * Mathf.Deg2Rad;
        movement.x = Mathf.Cos(angleRad);
        movement.y = Mathf.Sin(angleRad);
        Vector2 dashDirection = movement.normalized;

        // Perform the dash
        float startTime = Time.time;
        playerDashed.TriggerEvent(gameObject);
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        // End the dash
        isDashing = false;

        // Start cooldown
        float timeSinceLastDash = Time.time - lastDashTime;
        dashCooldownRemaining = Mathf.Max(0, dashCooldown - timeSinceLastDash);
        while (dashCooldownRemaining > 0f)
        {
            timeSinceLastDash = Time.time - lastDashTime;
            dashCooldownRemaining = Mathf.Max(0, dashCooldown - timeSinceLastDash);
            dashCooldownUpdated.TriggerEvent(gameObject);
            yield return null;
        }
        dashCooldownRemaining = 0.0f;
        dashCooldownUpdated.TriggerEvent(gameObject);
    }
}