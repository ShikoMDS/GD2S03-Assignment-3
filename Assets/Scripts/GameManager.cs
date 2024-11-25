using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerLives;
    public int maxLives = 3;
    public HealthUIManager healthUIManager; // Reference to the UI Health Manager
    public AdManager adManager; // Reference to the AdManager

    public Transform[] respawnPoints; // Array of respawn points for each stage
    private int currentStage = 0;

    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    public GameObject winScreen; // Win screen UI
    public GameObject loseScreen; // Lose screen UI

    private void Start()
    {
        ResetLives();

        // Ensure audio settings are applied
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            ApplyAudioSettings(audioManager);
            audioManager.PlayGameMusic();
        }
        else
        {
            Debug.LogError("AudioManager not found!");
        }

        int savedHighestStage = PlayerPrefs.GetInt("HighestStage", 0); // Default to 0 if no data exists
        if (savedHighestStage == 0)
        {
            savedHighestStage = 1;
            PlayerPrefs.SetInt("HighestStage", savedHighestStage);
            PlayerPrefs.Save();
            Debug.Log($"New highest stage achieved: {currentStage + 1}");
        }

        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
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

    public void ResetLives()
    {
        playerLives = maxLives; // Reset lives to maximum
        Debug.Log($"Lives reset to {playerLives}");

        // Initialize health UI
        if (healthUIManager != null)
        {
            healthUIManager.InitializeHealth(maxLives);
        }
        else
        {
            Debug.LogError("HealthUIManager not assigned.");
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        if (loseScreen != null) loseScreen.SetActive(false);

        playerLives--;

        if (playerLives <= 0)
        {
            Debug.LogError("Cannot respawn, player is out of lives.");
            GameOver(false); // Lose condition
            return;
        }

        if (healthUIManager != null)
        {
            healthUIManager.UpdateHealth(playerLives);
        }

        // Move player to respawn point
        if (respawnPoints.Length > currentStage)
        {
            player.transform.position = respawnPoints[currentStage].position;
            Debug.Log($"Player respawned. Lives remaining: {playerLives}");
        }
        else
        {
            Debug.LogError("No respawn point found for the current stage.");
        }
    }

    public void CompleteStage()
    {
        currentStage++;

        // Update the highest stage in PlayerPrefs if the current stage exceeds the saved highest stage
        int savedHighestStage = PlayerPrefs.GetInt("HighestStage", 0); // Default to 0 if no data exists
        if (currentStage + 1 > savedHighestStage)
        {
            if (currentStage + 1 >= 10)
            {
                PlayerPrefs.SetInt("HighestStage", 10);
                PlayerPrefs.Save();
                Debug.Log($"New highest stage achieved: 10");
            }
            else
            {
                PlayerPrefs.SetInt("HighestStage", currentStage + 1);
                PlayerPrefs.Save();
                Debug.Log($"New highest stage achieved: {currentStage + 1}");
            }
        }

        if (currentStage >= respawnPoints.Length)
        {
            Debug.Log("Game completed! No more stages.");
            GameOver(true); // Win condition
            return;
        }

        // Move player to the next stage's respawn point
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = respawnPoints[currentStage].position;
            CameraController.instance?.MoveToNextStage(currentStage);
            Debug.Log($"Stage {currentStage} complete. Player moved to the next stage.");
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }

    private void GameOver(bool hasWon)
    {
        if (hasWon)
        {
            Debug.Log("Player has won!");
            if (winScreen != null) winScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Player has lost!");

            if (loseScreen != null) loseScreen.SetActive(true);

            AdManager adManager = FindObjectOfType<AdManager>();
            if (adManager != null && playerLives <= 0 && !adManager.RewardAdUsed())
            {
                Button continueButton = loseScreen.GetComponentInChildren<Button>();
                if (continueButton != null)
                {
                    continueButton.gameObject.SetActive(true); // Show the Continue button
                }
            }
        }

        Time.timeScale = 0f; // Pause the game
    }

    public void OnRewardAdButtonClicked()
    {
        if (adManager != null && !adManager.RewardAdUsed())
        {
            adManager.ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not available or has already been used.");
        }
    }


    public void GiveExtraLife()
    {
        playerLives = 1; // Grant 1 life
        Debug.Log("Player granted an extra life!");

        // Respawn the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && respawnPoints.Length > currentStage)
        {
            player.transform.position = respawnPoints[currentStage].position;
            Debug.Log("Player respawned with an extra life.");
        }
        else
        {
            Debug.LogError("Player or respawn point not found.");
        }

        // Update health UI
        if (healthUIManager != null)
        {
            healthUIManager.UpdateHealth(playerLives);
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Menu");
    }
}
