using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cameraComponent;
    public float zoomOutDelay = 2f; // Delay before starting the zoom-out
    public float zoomOutDuration = 2f; // Duration of the zoom-out effect
    public float closeUpDistance = 5f; // Distance for the close-up effect
    public float zoomOutDistance = 20f; // Distance for the zoomed-out effect
    public float minFOV = 30f; // Minimum Field of View for close-up
    public float maxFOV = 60f; // Maximum Field of View for zoomed-out

    private Vector3 targetPosition;
    private float targetFOV;
    private Vector3 initialCameraPosition;

    void Start()
    {
        if (cameraComponent == null || cameraComponent.orthographic)
        {
            Debug.LogError("Main camera is either not found or not perspective. Ensure there's a perspective camera with the 'MainCamera' tag.");
        }
    }

    public void SetCameraTarget(Vector3 playerPosition, Vector3 mapCenter, float mapWidth, float mapHeight)
    {
        // Set initial position and FOV to close-up
        initialCameraPosition = new Vector3(playerPosition.x, playerPosition.y, -closeUpDistance);
        cameraComponent.transform.position = initialCameraPosition;
        cameraComponent.fieldOfView = minFOV;

        // Calculate the required FOV to fit the entire map
        float aspectRatio = cameraComponent.aspect; // Aspect ratio of the camera
        float mapDiagonal = Mathf.Sqrt(mapWidth * mapWidth + mapHeight * mapHeight);
        float requiredFOV = 2 * Mathf.Atan(mapDiagonal / (2 * zoomOutDistance)) * Mathf.Rad2Deg;

        targetPosition = new Vector3(mapCenter.x, mapCenter.y, -zoomOutDistance);
        targetFOV = Mathf.Clamp(requiredFOV, minFOV, maxFOV);

        StartCoroutine(ZoomOutRoutine(playerPosition));
    }

    private IEnumerator ZoomOutRoutine(Vector3 playerPositon)
    {
        // Wait for the delay
        yield return new WaitForSeconds(zoomOutDelay);

        // Smoothly transition to the target position and FOV
        Vector3 startPosition = cameraComponent.transform.position;
        float startFOV = cameraComponent.fieldOfView;

        float elapsedTime = 0f;

        while (elapsedTime < zoomOutDuration)
        {
            float t = elapsedTime / zoomOutDuration;
            cameraComponent.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            cameraComponent.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            adjustCameraToKeepPlayerInView(playerPositon);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraComponent.transform.position = targetPosition; // Ensure the camera ends at the final position
        cameraComponent.fieldOfView = targetFOV; // Ensure the camera ends at the final FOV
        Debug.Log("Zoom out complete.");
    }

    private void adjustCameraToKeepPlayerInView(Vector3 playerPositon)
    {
        // Get the camera's position and field of view
        Vector3 cameraPosition = cameraComponent.transform.position;
        float currentFOV = cameraComponent.fieldOfView;

        // Calculate the maximum viewable distance from the camera (half the width of the view frustum)
        float maxDistance = Mathf.Tan(currentFOV * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(cameraPosition.z);

        // Calculate the camera's new position to ensure the player remains in view
        Vector3 directionToPlayer = playerPositon - cameraPosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > maxDistance)
        {
            // Adjust camera position to keep player in view
            Vector3 newCameraPosition = playerPositon - directionToPlayer.normalized * maxDistance;
            cameraComponent.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, cameraPosition.z);
        }
    }
}
