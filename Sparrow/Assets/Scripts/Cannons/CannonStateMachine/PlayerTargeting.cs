using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public GameObject target;
    private bool _targetWasClicked = false;
    private string PLAYER_TAG = "Player";

    void Start()
    {
        target = null;
    }

    void Update()
    {
        
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
        shipConfig.UpdateCannonTarget(target);
    }
}
