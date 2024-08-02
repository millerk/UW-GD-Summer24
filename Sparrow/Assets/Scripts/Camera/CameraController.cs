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
    private Vector3 velocity = Vector3.zero; // For SmoothDamp
    private float fovVelocity = 0f; // For SmoothDamp

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

        // Clamp the FOV to be within min and max FOV
        targetFOV = Mathf.Clamp(requiredFOV, minFOV, maxFOV);

        targetPosition = new Vector3(mapCenter.x, mapCenter.y, -zoomOutDistance);

        StartCoroutine(ZoomOutRoutine(playerPosition));
    }

    private IEnumerator ZoomOutRoutine(Vector3 playerPosition)
    {
        // Wait for the delay
        yield return new WaitForSeconds(zoomOutDelay);

        Vector3 startPosition = cameraComponent.transform.position;
        float startFOV = cameraComponent.fieldOfView;

        float elapsedTime = 0f;

        while (elapsedTime < zoomOutDuration)
        {
            float t = elapsedTime / zoomOutDuration;

            // Smoothly transition the camera's position and FOV
            cameraComponent.transform.position = Vector3.SmoothDamp(
                cameraComponent.transform.position,
                targetPosition,
                ref velocity,
                zoomOutDuration - elapsedTime
            );

            cameraComponent.fieldOfView = Mathf.SmoothDamp(
                cameraComponent.fieldOfView,
                targetFOV,
                ref fovVelocity,
                zoomOutDuration - elapsedTime
            );

            // Ensure FOV stays within boundaries during transition
            cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minFOV, maxFOV);

            adjustCameraToKeepPlayerInView(playerPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are applied
        cameraComponent.transform.position = targetPosition;
        cameraComponent.fieldOfView = targetFOV;

        Debug.Log("Zoom out complete.");
    }

    private void adjustCameraToKeepPlayerInView(Vector3 playerPosition)
    {
        // Get the camera's position and field of view
        Vector3 cameraPosition = cameraComponent.transform.position;
        float currentFOV = cameraComponent.fieldOfView;

        // Calculate the maximum viewable distance from the camera (half the width of the view frustum)
        float maxDistance = Mathf.Tan(currentFOV * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(cameraPosition.z);

        // Calculate the camera's new position to ensure the player remains in view
        Vector3 directionToPlayer = playerPosition - cameraPosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > maxDistance)
        {
            // Adjust camera position to keep player in view, but limit adjustment to avoid abrupt changes
            Vector3 newCameraPosition = playerPosition - directionToPlayer.normalized * maxDistance;
            cameraComponent.transform.position = Vector3.Lerp(
                cameraComponent.transform.position,
                new Vector3(newCameraPosition.x, newCameraPosition.y, cameraComponent.transform.position.z),
                Time.deltaTime * 5f // Adjust this multiplier to control smoothness
            );
        }
    }
}
