using UnityEngine;

public class PlayerTargetWithAim : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public Cannon cannon;
    public GameObject target;
    private bool _targetWasClicked = false;
    private string PLAYER_TAG = "Player";
    private GameObject _predictedTarget; // Dummy target object

    void Start()
    {
        target = null;
        // Create or assign the dummy target object
        _predictedTarget = new GameObject("PredictedTarget");
        _predictedTarget.SetActive(false); // Make it invisible
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
        if (target == null && other.CompareTag(PLAYER_TAG))
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
        if (target != null && cannon != null)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                Vector3 predictedPosition = PredictPosition(targetRb);
                _predictedTarget.transform.position = predictedPosition;
                _predictedTarget.SetActive(true);
                shipConfig.UpdateCannonTarget(_predictedTarget);
            }
        }
        else
        {
            // If no target, deactivate the dummy target
            _predictedTarget.SetActive(false);
            shipConfig.UpdateCannonTarget(null); // Handle no target situation
        }
    }

    Vector3 PredictPosition(Rigidbody2D targetRb)
    {

        Vector3 targetPosition = targetRb.transform.position;
        Vector3 velocity = targetRb.velocity;
        float projectileSpeed = cannon.projectileSpeed;

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Calculate the time to impact
        float timeToImpact = distanceToTarget / projectileSpeed;

        //great for melee
        //transform.position = Vector3.Lerp(transform.position, targetPosition + new Vector3(velocity.x, velocity.y, 0f) * 0.5f, Time.deltaTime * 1f);

        // Predict the future position of the target
        Vector3 predictedPosition = targetPosition + velocity * timeToImpact;

        return predictedPosition;
    }
}