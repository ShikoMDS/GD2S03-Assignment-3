using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    private void Start()
    {
        // Load saved settings or set default values
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // Add listeners to UI elements
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicMuteToggle.onValueChanged.AddListener(ToggleMusicMute);
        sfxMuteToggle.onValueChanged.AddListener(ToggleSFXMute);
    }

    private void SetMusicVolume(float volume)
    {
        if (!musicMuteToggle.isOn) // Only set volume if not muted
        {
            // Replace with actual code to set music volume, e.g., through an AudioMixer
            AudioListener.volume = volume; // Example only
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        if (!sfxMuteToggle.isOn) // Only set volume if not muted
        {
            // Replace with actual code to set SFX volume, e.g., through an AudioMixer
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void ToggleMusicMute(bool isMuted)
    {
        if (isMuted)
        {
            // Set music volume to 0 if muted
            AudioListener.volume = 0;
        }
        else
        {
            // Restore volume from slider
            SetMusicVolume(musicSlider.value);
        }
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
    }

    private void ToggleSFXMute(bool isMuted)
    {
        if (isMuted)
        {
            // Set SFX volume to 0 if muted
        }
        else
        {
            // Restore SFX volume from slider
            SetSFXVolume(sfxSlider.value);
        }
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);
    }

    private void OnDestroy()
    {
        // Remove listeners when script is destroyed to avoid memory leaks
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        musicMuteToggle.onValueChanged.RemoveListener(ToggleMusicMute);
        sfxMuteToggle.onValueChanged.RemoveListener(ToggleSFXMute);
    }
}
