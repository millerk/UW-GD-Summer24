using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public GameObject target;
    private bool _targetWasClicked = false;
    private string PLAYER_TAG = "Player";
    public float targetingDelay = 5.0f; // Delay in seconds before allowing targeting
    private bool canTarget = false;

    void Start()
    {
        target = null;
        StartCoroutine(DelayTargeting());
    }

    void Update()
    {
        // Add any additional logic needed for the Update method
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (ReferenceEquals(target, enemy))
        {
            UpdateTarget(null, false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canTarget && target == null && other.CompareTag(PLAYER_TAG))
        {
            UpdateTarget(other.gameObject, false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ReferenceEquals(other.gameObject, target) && !_targetWasClicked)
        {
            UpdateTarget(null, false);
        }
    }

    private void UpdateTarget(GameObject newTarget, bool wasClicked)
    {
        target = newTarget;
        _targetWasClicked = wasClicked;
        shipConfig.UpdateCannonTarget(target);
        // Great for melee
        // transform.position = Vector3.Lerp(transform.position, targetPosition + new Vector3(velocity.x, velocity.y, 0f) * 0.5f, Time.deltaTime * 1f);
    }

    private IEnumerator DelayTargeting()
    {
        // Disable targeting initially
        canTarget = false;

        // Wait for the specified delay
        yield return new WaitForSeconds(targetingDelay);

        // Enable targeting after delay
        canTarget = true;
    }
}