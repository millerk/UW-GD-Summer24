using UnityEngine;
using UnityEngine.UI;

public class AudioMuteToggle : MonoBehaviour
{
    public Button muteButton;  // Reference to the UI button
    public Text buttonText;    // Reference to the Text component of the button
    private bool isMuted = false;  // Track mute state

    void Start()
    {
        if (muteButton != null)
        {
            // Assign the ToggleMute method to the button's onClick event
            muteButton.onClick.AddListener(ToggleMute);

            // Set initial button text based on the initial mute state
            UpdateButtonText();
        }
        else
        {
            Debug.LogError("MuteButton is not assigned.");
        }
    }

    // Method to toggle the mute state
    void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;

        // Update the button text based on the new mute state
        UpdateButtonText();
    }

    // Method to update the button text
    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = isMuted ? "Unmute" : "Mute";
        }
        else
        {
            Debug.LogError("ButtonText is not assigned.");
        }
    }
}