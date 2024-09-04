using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Required for the Slider component

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioMixMode mixMode;
    [SerializeField]
    private Slider volumeSlider; // Reference to the Slider component

    private const string VolumeKey = "Volume";

    void Start()
    {
        // Load the volume from PlayerPrefs or set to default value if not set
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f); // Default to 1 if not set
        volumeSlider.value = savedVolume;
        UpdateMixerVolume(savedVolume);
    }

    public void OnChangeSlider(float value)
    {
        UpdateMixerVolume(value);

        // Save the volume level in PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
    }

    private void UpdateMixerVolume(float value)
    {
        switch (mixMode)
        {
            case AudioMixMode.LogarithmicMixerVolume:
                // Convert slider value to logarithmic scale for volume adjustment
                float dbValue = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20; // Avoid log10(0)
                mixer.SetFloat("Volume", dbValue);
                break;
        }
    }

    public enum AudioMixMode
    {
        LogarithmicMixerVolume
    }
}