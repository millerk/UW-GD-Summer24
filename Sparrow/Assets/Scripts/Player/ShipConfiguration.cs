using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConfiguration : MonoBehaviour
{
    public GameObject[] anchorPoints;
    public Cannon[] cannonDefs;
    public GameObject cannonTemplate;
    private List<GameObject> _cannons = new();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cannonDefs.Length; i++)
        {
            Cannon _cannon = cannonDefs[i];
            if (_cannon != null)
            {
                GameObject _anchorPoint = anchorPoints[i];
                GameObject _cannonObj = Instantiate(cannonTemplate, _anchorPoint.transform.position, _anchorPoint.transform.rotation);
                _cannonObj.transform.parent = _anchorPoint.transform;
                _cannonObj.GetComponent<CannonBuilder>().cannonDef = _cannon;
                _cannons.Add(_cannonObj);
            }
        }
    }
}
