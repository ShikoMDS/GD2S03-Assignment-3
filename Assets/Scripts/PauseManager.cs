using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance; // Singleton instance

    public GameObject pauseScreen; // Reference to the pause screen UI

    private bool isPaused = false; // Track whether the game is paused

    private void Awake()
    {
        // Ensure only one instance of PauseManager exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this; // Set this as the active instance
    }

    private void Start()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false); // Ensure pause screen is hidden at start
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true); // Show the pause screen
        }

        Time.timeScale = 0f; // Freeze the game
        isPaused = true;
        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false); // Hide the pause screen
        }

        Time.timeScale = 1f; // Resume the game
        isPaused = false;
        Debug.Log("Game resumed.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene

        // Reinitialize UI elements after loading the scene
        StartCoroutine(ReinitializeUI());
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Ensure time is running
        SceneManager.LoadScene("Menu"); // Load the main menu scene
    }

    private IEnumerator ReinitializeUI()
    {
        yield return new WaitForSeconds(0.1f); // Wait for scene to load

        pauseScreen = GameObject.Find("Pause Screen"); // Find pause screen again
        if (pauseScreen == null)
        {
            Debug.LogError("Pause screen not found after restart.");
        }
    }
}
