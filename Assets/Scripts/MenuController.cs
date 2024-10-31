using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;
    public GameObject optionsPanel;

    [Header("Volume Controls")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    private void Start()
    {
        ShowMainMenu();
        LoadVolumeSettings();
    }

    // Main Menu Button Functions
    public void PlayGame()
    {
        // Load the game scene here
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1Scene");
    }

    public void ShowInstructions()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // Back Button Function (for both Instructions and Options)
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    // Volume and Mute Control Functions
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        // Add logic to control actual audio source volume here
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        // Add logic to control actual SFX volume here
    }

    public void ToggleMusicMute(bool isMuted)
    {
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        // Add logic to mute/unmute music audio source
    }

    public void ToggleSFXMute(bool isMuted)
    {
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);
        // Add logic to mute/unmute SFX audio source
    }

    private void LoadVolumeSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }
}
