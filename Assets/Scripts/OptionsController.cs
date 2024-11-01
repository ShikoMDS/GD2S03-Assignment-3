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
    private float previousMusicVolume = 0.5f; // Default previous volume for Music
    private float previousSFXVolume = 0.5f;   // Default previous volume for SFX

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        // Load saved settings and initialize toggle states based on slider values
        previousMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        previousSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSlider.value = previousMusicVolume;
        sfxSlider.value = previousSFXVolume;
        musicMuteToggle.isOn = previousMusicVolume > 0;
        sfxMuteToggle.isOn = previousSFXVolume > 0;

        // Set initial audio settings
        audioManager.SetMusicVolume(musicSlider.value);
        audioManager.SetSFXVolume(sfxSlider.value);
        audioManager.ToggleMusicMute(musicMuteToggle.isOn);
        audioManager.ToggleSFXMute(sfxMuteToggle.isOn);

        // Add listeners for sliders and toggles
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggleChanged);
        sfxMuteToggle.onValueChanged.AddListener(OnSFXMuteToggleChanged);
    }

    private void OnMusicSliderChanged(float value)
    {
        if (value == 0)
        {
            musicMuteToggle.isOn = false; // Sync mute toggle to "off" if slider is 0
        }
        else
        {
            musicMuteToggle.isOn = true;  // Turn sound on if slider is above 0
            previousMusicVolume = value;  // Update previous volume
        }
        audioManager.SetMusicVolume(value);
    }

    private void OnSFXSliderChanged(float value)
    {
        if (value == 0)
        {
            sfxMuteToggle.isOn = false; // Sync mute toggle to "off" if slider is 0
        }
        else
        {
            sfxMuteToggle.isOn = true;  // Turn sound on if slider is above 0
            previousSFXVolume = value;  // Update previous volume
        }
        audioManager.SetSFXVolume(value);
    }

    private void OnMusicMuteToggleChanged(bool isSoundOn)
    {
        if (isSoundOn)
        {
            musicSlider.value = previousMusicVolume; // Restore previous volume when unmuted
        }
        else
        {
            previousMusicVolume = musicSlider.value; // Store current volume before muting
            musicSlider.value = 0; // Set volume to 0 when muted
        }
        audioManager.ToggleMusicMute(isSoundOn);
    }

    private void OnSFXMuteToggleChanged(bool isSoundOn)
    {
        if (isSoundOn)
        {
            sfxSlider.value = previousSFXVolume; // Restore previous volume when unmuted
        }
        else
        {
            previousSFXVolume = sfxSlider.value; // Store current volume before muting
            sfxSlider.value = 0; // Set volume to 0 when muted
        }
        audioManager.ToggleSFXMute(isSoundOn);
    }
}
