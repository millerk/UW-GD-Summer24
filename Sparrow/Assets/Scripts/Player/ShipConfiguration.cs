using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipConfiguration : MonoBehaviour
{
    public GameObject[] anchorPoints;
    public Cannon[] cannonDefs;
    public GameObject cannonTemplate;
    public Rigidbody2D shipRb;
    private List<GameObject> _cannons = new();
    private GameObject _target = null;
    private bool _targetWasClicked = false;
    private string ENEMY_TAG = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        
        float _maxDistance = 0f;
        for (int i = 0; i < cannonDefs.Length; i++)
        {
            Cannon _cannon = cannonDefs[i];
            if (_cannon != null)
            {
                // Create cannon at anchor point from the defintion obj
                GameObject _anchorPoint = anchorPoints[i];
                GameObject _cannonObj = Instantiate(cannonTemplate, _anchorPoint.transform.position, _anchorPoint.transform.rotation);
                CannonContainer container = _cannonObj.GetComponent<CannonContainer>();
                container.cannonDef = _cannon;
                container.shipRb = shipRb;
                _cannonObj.transform.parent = _anchorPoint.transform;
                _cannons.Add(_cannonObj);

                _maxDistance = Math.Max(_maxDistance, _cannon.attackDistance);
            }
        }
        CircleCollider2D _autoTargetCollider = gameObject.GetComponent<CircleCollider2D>();
        _autoTargetCollider.radius = _maxDistance - 0.05f; // a little wiggle room so that we don't auto-target things just on the edge of range
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.transform.gameObject.CompareTag(ENEMY_TAG))
        {
             _targetWasClicked = true;
            UpdateTarget(hit.transform.gameObject);
        }
        else
        {
            // Clicked somewhere else on screen, deselect the target
            UpdateTarget(null);
            _targetWasClicked = false;
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger");
        if (_target == null && other.CompareTag(ENEMY_TAG))
        {
            Debug.Log("Updating target to " + other.name);
            UpdateTarget(other.gameObject);
            _targetWasClicked = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If player clicked to target we should always respect it
        // If target was selected by auto-fire then deselect when we go out of range
        if (GameObject.ReferenceEquals(other.gameObject, _target) && !_targetWasClicked)
        {
            UpdateTarget(null);
        }
    }

    void UpdateTarget(GameObject target)
    {
        _target = target;
        for (int i = 0; i < _cannons.Count; i++)
        {
            CannonContainer container = _cannons[i].GetComponent<CannonContainer>();
            container.cannon.GetComponent<CannonStateMachine>().SetTarget(_target);
        }
    }
}
