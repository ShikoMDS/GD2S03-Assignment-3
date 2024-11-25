using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;
    public GameObject optionsPanel;

    public TMP_Text highestStageText; // Assign this in the Unity Inspector

    private AudioManager audioManager;

    private const string HighestStageKey = "HighestStage";

    private void Start()
    {
        // Load and display the highest stage on the menu
        int highestStage = PlayerPrefs.GetInt(HighestStageKey, 0); // Default is 0
        UpdateHighestStageText(highestStage);

        Debug.Log("Menu loaded. Highest Stage: " + highestStage);

        ShowMainMenu();
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager != null)
        {
            // Apply audio settings before playing menu music
            ApplyAudioSettings(audioManager);
            audioManager.PlayMenuMusic();
        }
        else
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    private void ApplyAudioSettings(AudioManager audioManager)
    {
        // Load audio settings from PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bool musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bool sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // Apply settings to AudioManager
        audioManager.musicSource.volume = musicVolume;
        audioManager.musicSource.mute = musicMuted;
        audioManager.sfxSource.volume = sfxVolume;
        audioManager.sfxSource.mute = sfxMuted;
    }

    // Main Menu Button Functions
    public void PlayGame()
    {
        Debug.Log("Initializing game audio sources...");

        // Ensure audio sources are reinitialized for the game
        SceneManager.LoadScene("Game");
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

    private void UpdateHighestStageText(int highestStage)
    {
        if (highestStageText != null)
        {
            highestStageText.text = $"Highest Stage: {highestStage}";
        }
        else
        {
            Debug.LogError("HighestStageText UI element is not assigned.");
        }
    }
}
