using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionScript : MonoBehaviour
{
    public Dropdown resolutionDropdown; // Dropdown component to show the resolutions
    Resolution[] resolutions; // Array to hold the available screen resolutions

    void Start()
    {
        // Fetch all available screen resolutions
        resolutions = Screen.resolutions;

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
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // Check if the index is valid
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}