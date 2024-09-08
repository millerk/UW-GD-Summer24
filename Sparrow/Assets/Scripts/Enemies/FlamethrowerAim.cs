using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAim : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public Cannon cannon;
    private bool _targetWasClicked = false;
    private GameObject target; // Target is no longer public
    private GameObject _dummyTarget; // Dummy target object for aiming

    // Adjust this distance to make the dummy target closer
    public float dummyTargetDistance = 10f; // Set a default closer distance

    void Start()
    {
        // Find the player object
        target = GameObject.FindGameObjectWithTag("Player");

        // Create a dummy target object
        _dummyTarget = new GameObject("DummyTarget");
        _dummyTarget.SetActive(false); // Make it invisible
    }

    void Update()
    {
        if (cannon != null)
        {
            // Aim the dummy target straight ahead
            Vector3 forwardDirection = transform.up; // Assuming 'up' is the forward direction
            _dummyTarget.transform.position = transform.position + forwardDirection * dummyTargetDistance; // Use the closer distance
            _dummyTarget.SetActive(true); // Ensure it is active

            // Update the cannon's target to the dummy target
            shipConfig.UpdateCannonTarget(_dummyTarget);
        }
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (ReferenceEquals(target, enemy))
        {
            UpdateTarget(null, false);
        }
    }

    // Enemy ship enters our shooting range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (target == null)
        {
            UpdateTarget(other.gameObject, false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If player clicked to target we should always respect it
        // If target was selected by auto-fire then deselect when we go out of range
        if (ReferenceEquals(other.gameObject, target) && !_targetWasClicked)
        {
            UpdateTarget(null, false);
        }
    }

    private void UpdateTarget(GameObject newTarget, bool wasClicked)
    {
        target = newTarget;
        _targetWasClicked = wasClicked;

        if (cannon != null && shipConfig != null)
        {
            // Update the dummy target's position
            Vector3 forwardDirection = transform.up; // Assuming 'up' is the forward direction
            _dummyTarget.transform.position = transform.position + forwardDirection * dummyTargetDistance; // Use the closer distance
            _dummyTarget.SetActive(true); // Ensure it is active

            // Update the cannon's target to the dummy target
            shipConfig.UpdateCannonTarget(_dummyTarget);
        }
        else
        {
            if (shipConfig == null)
                Debug.LogError("ShipConfiguration is not assigned!");

            if (cannon == null)
                Debug.LogError("Cannon is not assigned!");

            // If no target, reset the dummy target
            _dummyTarget.SetActive(false);
            shipConfig.UpdateCannonTarget(null);
        }
    }

    void OnDestroy()
    {
        // Clean up dummy target object when this script is destroyed
        if (_dummyTarget != null)
        {
            Destroy(_dummyTarget);
        }
    }
}