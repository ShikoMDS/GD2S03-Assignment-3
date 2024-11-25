using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;
    public GameObject optionsPanel;

    private AudioManager audioManager;

    private void Start()
    {
        ShowMainMenu();
        audioManager = FindObjectOfType<AudioManager>();

        // Start playing the menu music
        audioManager.PlayMenuMusic();
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
}
