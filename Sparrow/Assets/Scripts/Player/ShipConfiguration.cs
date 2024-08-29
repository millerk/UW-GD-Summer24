using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipConfiguration : MonoBehaviour
{
    public GameObject[] anchorPoints;
    public Cannon[] cannonDefs;
    public GameObject cannonTemplate;
    public Rigidbody2D shipRb;
    private List<GameObject> _cannons = new();

    public static string PLAYER_CANNON_DEF = "Player Cannon Configuration";

    // Start is called before the first frame update
    void Start()
    {
        // If we have transitioned from previous screen, use the players existing cannon setup, otherwise
        // reset to default (definied in editor Player ship prefab)
        if (GlobalVariables.Get<Cannon[]>(PLAYER_CANNON_DEF) != null)
        {
            cannonDefs = GlobalVariables.Get<Cannon[]>(PLAYER_CANNON_DEF);
        }

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

    public void UpdateCannonTarget(GameObject target)
    {
        for (int i = 0; i < _cannons.Count; i++)
        {
            CannonContainer container = _cannons[i].GetComponent<CannonContainer>();
            container.cannon.GetComponent<CannonStateMachine>().SetTarget(target);
        }
    }

    public void OnLevelComplete(GameObject source)
    {
        GlobalVariables.Set(PLAYER_CANNON_DEF, cannonDefs);
    }
}
