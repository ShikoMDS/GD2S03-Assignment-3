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

    private AudioManager audioManager;
    private float previousMusicVolume = 0.5f; // Default previous value for Music
    private float previousSFXVolume = 0.5f;   // Default previous value for SFX

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        // Load saved settings
        previousMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        previousSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSlider.value = previousMusicVolume;
        sfxSlider.value = previousSFXVolume;
        musicMuteToggle.isOn = previousMusicVolume > 0;
        sfxMuteToggle.isOn = previousSFXVolume > 0;

        // Initial audio settings
        audioManager.SetMusicVolume(musicSlider.value);
        audioManager.SetSFXVolume(sfxSlider.value);
        audioManager.ToggleMusicMute(!musicMuteToggle.isOn);
        audioManager.ToggleSFXMute(!sfxMuteToggle.isOn);

        // Add listeners
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggleChanged);
        sfxMuteToggle.onValueChanged.AddListener(OnSFXMuteToggleChanged);
    }

    // Called when the Music slider is adjusted
    private void OnMusicSliderChanged(float value)
    {
        if (value == 0)
        {
            musicMuteToggle.isOn = false; // Sync mute toggle if slider is 0
        }
        else
        {
            musicMuteToggle.isOn = true; // Unmute if slider is above 0
            previousMusicVolume = value; // Update previous volume
        }
        audioManager.SetMusicVolume(value);
    }

    // Called when the SFX slider is adjusted
    private void OnSFXSliderChanged(float value)
    {
        if (value == 0)
        {
            sfxMuteToggle.isOn = false; // Sync mute toggle if slider is 0
        }
        else
        {
            sfxMuteToggle.isOn = true; // Unmute if slider is above 0
            previousSFXVolume = value; // Update previous volume
        }
        audioManager.SetSFXVolume(value);
    }

    // Called when the Music mute toggle is clicked
    private void OnMusicMuteToggleChanged(bool isSoundOn)
    {
        if (isSoundOn)
        {
            musicSlider.value = previousMusicVolume; // Restore previous volume
        }
        else
        {
            previousMusicVolume = musicSlider.value; // Store current volume
            musicSlider.value = 0; // Set volume to 0
        }
        audioManager.ToggleMusicMute(!isSoundOn);
    }

    // Called when the SFX mute toggle is clicked
    private void OnSFXMuteToggleChanged(bool isSoundOn)
    {
        if (isSoundOn)
        {
            sfxSlider.value = previousSFXVolume; // Restore previous volume
        }
        else
        {
            previousSFXVolume = sfxSlider.value; // Store current volume
            sfxSlider.value = 0; // Set volume to 0
        }
        audioManager.ToggleSFXMute(!isSoundOn);
    }
}
