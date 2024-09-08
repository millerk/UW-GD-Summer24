using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionScript : MonoBehaviour
{
    public Dropdown resolutionDropdown; // Dropdown component to show the resolutions
    private List<Resolution> resolutions = new List<Resolution>(); // List to hold the available screen resolutions

    void Start()
    {
        LoadResolutions();
    }

    private void LoadResolutions()
    {
        // Clear the existing resolutions list
        resolutions.Clear();

        // Fetch all available screen resolutions
        Resolution[] allResolutions = Screen.resolutions;

        // Populate the list with screen resolutions
        foreach (Resolution resolution in allResolutions)
        {
            resolutions.Add(resolution);
        }

        // Clear any existing options in the dropdown
        resolutionDropdown.ClearOptions();

        // Create a list to store options for the dropdown
        List<string> options = new List<string>();

        // Populate the options list with screen resolutions
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);
        }

        // Add options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Optionally, set the default resolution to the current screen resolution
        int currentResolutionIndex = options.IndexOf(Screen.currentResolution.width + " x " + Screen.currentResolution.height);
        if (currentResolutionIndex != -1)
        {
            resolutionDropdown.value = currentResolutionIndex;
        }

        // Ensure the dropdown triggers the resolution change when a new option is selected
        resolutionDropdown.onValueChanged.RemoveListener(SetResolution); // Remove existing listeners to avoid duplicates
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // Check if the index is valid
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Count)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}