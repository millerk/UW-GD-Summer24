using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Use UnityEngine.UI if you are using the UI Slider component for the volume control

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer; // Ensure the AudioMixer is assigned in the inspector
    [SerializeField]
    private AudioMixMode mixMode;

    [SerializeField]
    private Slider volumeSlider; // The Slider UI component that controls the volume

    private const string VolumeKey = "Volume";
    private const float DefaultVolume = 1.0f; // Default volume if not set in PlayerPrefs

    private void Start()
    {
        // Load the saved volume value from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);

        // Set the slider value to the saved volume value
        volumeSlider.value = savedVolume;

        // Apply the volume setting based on the saved value
        OnChangeSlider(savedVolume);

        // Add a listener to the slider to call OnChangeSlider when the value changes
        volumeSlider.onValueChanged.AddListener(OnChangeSlider);
    }

    public void OnChangeSlider(float value)
    {
        // Apply the slider value based on the mix mode
        switch (mixMode)
        {
            case AudioMixMode.LinearMixerVolume:
                mixer.SetFloat("Volume", -80 + value * 80);
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                mixer.SetFloat("Volume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
                break;
        }

        // Save the volume value to PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
    }
}

public enum AudioMixMode
{
    LinearAudioSourceVolume,
    LinearMixerVolume,
    LogrithmicMixerVolume,
}