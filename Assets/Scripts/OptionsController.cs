using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public AudioManager audioManager;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    private void Start()
    {
        // Find AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();

        // Initialize sliders and toggles based on saved PlayerPrefs
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 0;
        sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 0;

        // Apply settings to the AudioManager
        ApplySettings();
    }

    public void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(value);
        }
    }

    public void OnSFXSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(value);
        }
    }

    public void OnMusicMuteToggled(bool isMuted)
    {
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        if (audioManager != null)
        {
            audioManager.musicSource.mute = isMuted;
        }
    }

    public void OnSFXMuteToggled(bool isMuted)
    {
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);
        if (audioManager != null)
        {
            audioManager.sfxSource.mute = isMuted;
        }
    }

    private void ApplySettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bool musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bool sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        if (audioManager != null)
        {
            audioManager.SetMusicVolume(musicVolume);
            audioManager.SetSFXVolume(sfxVolume);
            audioManager.musicSource.mute = musicMuted;
            audioManager.sfxSource.mute = sfxMuted;
        }
    }
}
