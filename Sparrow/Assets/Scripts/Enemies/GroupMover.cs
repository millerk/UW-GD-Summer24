using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMover : MonoBehaviour
{
    private Vector3[] initialLocalPositions;

    private void Start()
    {
        // Store the initial relative positions
        int childCount = transform.childCount;
        initialLocalPositions = new Vector3[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            initialLocalPositions[i] = child.localPosition;
        }
    }

    private void Update()
    {
        // Example: Move the parent GameObject using input or any other method
        Vector3 newPosition = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime;
        transform.position = newPosition;

        // Adjust child positions to maintain relative positions
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = initialLocalPositions[i];
        }
    }
}