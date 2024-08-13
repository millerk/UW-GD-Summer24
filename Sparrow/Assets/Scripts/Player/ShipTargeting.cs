using UnityEngine;

public class ShipTargeting : MonoBehaviour
{
    public ShipConfiguration shipConfig;
    public GameObject target;
    private bool _targetWasClicked = false;
    private string ENEMY_TAG = "Enemy";

    void Start()
    {
        target = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused)
        {
            SetTargetOnMouseClick();
        }
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (ReferenceEquals(target, enemy))
        {
            UpdateTarget(null, false);
        }
    }

    void SetTargetOnMouseClick()
    {
        LayerMask enemyLayers = LayerMask.GetMask("Enemy");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, enemyLayers);
        if (hit.collider != null && hit.transform.gameObject.CompareTag(ENEMY_TAG))
        {
            UpdateTarget(hit.transform.gameObject, true);
        }
        else
        {
            // Clicked somewhere else on screen, deselect the target
            UpdateTarget(null, false);
        }   
    }

    // Enemy ship enters our shooting range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (target == null && other.CompareTag(ENEMY_TAG))
        {
            UpdateTarget(other.gameObject, false);
        }
    }

    // When player deselects a manual target but is still in range of a ship, turn auto attack back on
    void OnTriggerStay2D(Collider2D other)
    {
        if (target == null && other.CompareTag(ENEMY_TAG))
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
