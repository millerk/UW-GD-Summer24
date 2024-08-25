using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float cameraZoom = 10f; // Adjust as needed

    // Method to set camera target
    public void SetCameraTarget(Transform playerTransform, Vector3 mapCenter)
    {
        if (virtualCamera != null)
        {
            // Set the camera follow target to the player
            virtualCamera.Follow = playerTransform;

            // Adjust the camera zoom level
            virtualCamera.m_Lens.OrthographicSize = cameraZoom;

            // Optional: Center the camera on the map
            Vector3 cameraPosition = new Vector3(mapCenter.x, mapCenter.y, virtualCamera.transform.position.z);
            virtualCamera.transform.position = cameraPosition;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera reference is not set.");
        }
    }
}
