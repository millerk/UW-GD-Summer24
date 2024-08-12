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

    void SetTargetOnMouseClick()
    {
        LayerMask enemyLayers = LayerMask.GetMask("Enemy");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, enemyLayers);
        if (hit.collider != null && hit.transform.gameObject.CompareTag(ENEMY_TAG))
        {
            target = hit.transform.gameObject;
            shipConfig.UpdateTarget(target);
            _targetWasClicked = true;
        }
        else
        {
            // Clicked somewhere else on screen, deselect the target
            target = null;
            shipConfig.UpdateTarget(target);
            _targetWasClicked = false;
        }   
    }

    // Enemy ship enters our shooting range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (target == null && other.CompareTag(ENEMY_TAG))
        {   
            target = other.gameObject;
            shipConfig.UpdateTarget(target);
            _targetWasClicked = false;
        }
    }

    // When player deselects a manual target but is still in range of a ship, turn auto attack back on
    void OnTriggerStay2D(Collider2D other)
    {
        if (target == null && other.CompareTag(ENEMY_TAG))
        {
            target = other.gameObject;
            shipConfig.UpdateTarget(target);
            _targetWasClicked = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If player clicked to target we should always respect it
        // If target was selected by auto-fire then deselect when we go out of range
        if (ReferenceEquals(other.gameObject, target) && !_targetWasClicked)
        {
            target = null;
            shipConfig.UpdateTarget(target);
            _targetWasClicked = false;
        }
    }
}
