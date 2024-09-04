using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;
    [SerializeField] Slider volumeSlider;

    const string MIXER_VOLUME = "Volume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_EFFECT = "EffectVolume";

    // PlayerPrefs keys for slider values
    const string PREF_VOLUME = "VolumeSliderValue";
    const string PREF_MUSIC = "MusicSliderValue";
    const string PREF_EFFECT = "EffectSliderValue";

    void Awake()
    {
        // Load saved values
        LoadSettings();

        // Add listeners for slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectSlider.onValueChanged.AddListener(SetEffectVolume);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        SaveSettings();
    }

    void SetEffectVolume(float value)
    {
        mixer.SetFloat(MIXER_EFFECT, Mathf.Log10(value) * 20);
        SaveSettings();
    }

    void SetVolume(float value)
    {
        mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(value) * 20);
        SaveSettings();
    }

    void LoadSettings()
    {
        // Load saved values or use defaults if none found
        float volume = PlayerPrefs.GetFloat(PREF_VOLUME, 0.75f); // Default to 0.75
        float music = PlayerPrefs.GetFloat(PREF_MUSIC, 0.75f); // Default to 0.75
        float effect = PlayerPrefs.GetFloat(PREF_EFFECT, 0.75f); // Default to 0.75

        volumeSlider.value = volume;
        musicSlider.value = music;
        effectSlider.value = effect;

        // Apply values to the mixer
        mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(volume) * 20);
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(music) * 20);
        mixer.SetFloat(MIXER_EFFECT, Mathf.Log10(effect) * 20);
    }

    void SaveSettings()
    {
        // Save the slider values to PlayerPrefs
        PlayerPrefs.SetFloat(PREF_VOLUME, volumeSlider.value);
        PlayerPrefs.SetFloat(PREF_MUSIC, musicSlider.value);
        PlayerPrefs.SetFloat(PREF_EFFECT, effectSlider.value);

        // It's a good practice to call Save to ensure data is written to disk
        PlayerPrefs.Save();
    }
}