using UnityEngine;

public class PlayerTargetWithAim : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public Cannon cannon;
    private bool _targetWasClicked = false;
    private GameObject _predictedTarget; // Dummy target object
    private GameObject target; // Target is no longer public

    // New variables to adjust the prediction angle and distance
    public float predictionAdjustment = 0.0f; // Distance adjustment
    public float angleAdjustment = 0.0f; // Angle adjustment in degrees

    void Start()
    {
        // Create or assign the dummy target object
        _predictedTarget = new GameObject("PredictedTarget");
        _predictedTarget.SetActive(false); // Make it invisible

        // Find the player object
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            UpdateTarget(target, false);
        }
    }

    void Update()
    {
        if (target != null)
        {
            // If the target has a Rigidbody2D, predict its future position
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null && cannon != null)
            {
                Vector3 predictedPosition = PredictPosition(targetRb);
                _predictedTarget.transform.position = predictedPosition;
                _predictedTarget.SetActive(true); // Activate it if needed
                shipConfig.UpdateCannonTarget(_predictedTarget); // Pass the dummy target
            }
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

        if (target != null && cannon != null && shipConfig != null)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                Vector3 predictedPosition = PredictPosition(targetRb);
                _predictedTarget.transform.position = predictedPosition;
                _predictedTarget.SetActive(true);
                shipConfig.UpdateCannonTarget(_predictedTarget);
            }
            else
            {
                // If targetRb is null, handle this case appropriately
                Debug.LogError("Target Rigidbody2D not found!");
                _predictedTarget.SetActive(false);
                shipConfig.UpdateCannonTarget(null);
            }
        }
        else
        {
            if (shipConfig == null)
                Debug.LogError("ShipConfiguration is not assigned!");

            if (cannon == null)
                Debug.LogError("Cannon is not assigned!");

            // If no target, deactivate the dummy target
            _predictedTarget.SetActive(false);
            shipConfig.UpdateCannonTarget(null); // Handle no target situation
        }
    }

    Vector3 PredictPosition(Rigidbody2D targetRb)
    {
        Vector3 targetPosition = targetRb.transform.position;
        Vector3 targetVelocity = targetRb.velocity;
        float projectileSpeed = cannon.projectileSpeed;

        // Get the shooter's Rigidbody2D (assuming this script is attached to the shooter)
        Rigidbody2D shooterRb = GetComponent<Rigidbody2D>();
        if (shooterRb == null)
        {
            Debug.LogError("Shooter Rigidbody2D not found!");
            return targetPosition; // Fall back to the target's current position
        }
        Vector3 shooterVelocity = shooterRb.velocity;

        // Calculate the direction from the shooter to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Calculate the relative velocity
        Vector3 relativeVelocity = targetVelocity - shooterVelocity;

        // Calculate the time to impact
        float timeToImpact = distanceToTarget / projectileSpeed;

        // Adjust the time to impact if both are moving in the same direction
        float directionDotProduct = Vector3.Dot(directionToTarget.normalized, relativeVelocity.normalized);
        if (directionDotProduct > 0.9f) // They are moving in approximately the same direction
        {
            timeToImpact *= 0.5f; // Increase shooting speed by reducing time to impact
        }

        // Predict the future position of the target
        Vector3 predictedPosition = targetPosition + relativeVelocity * timeToImpact;

        // Apply manual adjustment to the prediction
        // Convert angle adjustment to radians and adjust position accordingly
        float angleInRadians = angleAdjustment * Mathf.Deg2Rad;
        Vector3 adjustedDirection = Quaternion.Euler(0, 0, angleAdjustment) * (predictedPosition - transform.position).normalized;
        predictedPosition = transform.position + adjustedDirection * distanceToTarget + (predictedPosition - transform.position).normalized * predictionAdjustment;

        return predictedPosition;
    }
}