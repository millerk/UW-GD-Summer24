using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoarder : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;

    // Points of the polygon. You will define these in the Unity Inspector or programmatically.
    public Vector2[] points = new Vector2[4];

    void Start()
    {
        // Get the PolygonCollider2D component
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Initialize the collider with the specified points
        UpdateCollider();
    }

    void UpdateCollider()
    {
        if (polygonCollider != null)
        {
            // Set the points of the PolygonCollider2D
            polygonCollider.points = points;
        }
    }

    // Call this method to update the collider's points dynamically
    public void SetBoundary(Vector2[] newPoints)
    {
        points = newPoints;
        UpdateCollider();
    }
}