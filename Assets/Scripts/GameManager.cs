using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public int playerLives;
    public int maxLives = 3;
    public HealthUIManager healthUIManager; // Reference to the UI Health Manager

    public Transform[] respawnPoints; // Array of respawn points for each stage
    private int currentStage = 0;

    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject); // Destroy the old instance
        }

        instance = this; // Set this as the active instance
        DontDestroyOnLoad(gameObject); // Persist this GameManager across scenes
    }

    private void Start()
    {
        // Reset lives and initialize the health UI when the game starts
        ResetLives();
        InitializeAudio();
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

    private void InitializeAudio()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);
            AudioManager.instance.PlayGameMusic();
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        playerLives--;

        if (playerLives <= 0)
        {
            Debug.LogError("Cannot respawn, player is out of lives.");
            GameOver();
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

        if (currentStage >= respawnPoints.Length)
        {
            Debug.Log("Game completed! No more stages.");
            GameOver(); // End the game
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

    private void GameOver()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopAllAudio(); // Stop all audio to prevent overlap
        }

        Debug.Log("Game Over! Returning to Menu...");
        SceneManager.LoadScene("Menu");

        // Reinitialize audio for the Menu after the scene loads
        StartCoroutine(ReinitializeMenuAudio());
    }

    private IEnumerator ReinitializeMenuAudio()
    {
        yield return new WaitForSeconds(0.1f); // Wait for the scene to load

        if (AudioManager.instance != null)
        {
            AudioSource menuMusicSource = GameObject.Find("Music")?.GetComponent<AudioSource>();
            AudioSource menuSfxSource = GameObject.Find("SFX")?.GetComponent<AudioSource>();

            if (menuMusicSource != null && menuSfxSource != null)
            {
                AudioManager.instance.SetAudioSources(menuMusicSource, menuSfxSource);
                AudioManager.instance.PlayMenuMusic();
            }
            else
            {
                Debug.LogError("Menu audio sources not found.");
            }
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }

    public IEnumerator ReinitializeGameAudio()
    {
        yield return new WaitForSeconds(0.1f); // Wait for the scene to load

        if (AudioManager.instance != null)
        {
            AudioSource gameMusicSource = GameObject.Find("Game Music")?.GetComponent<AudioSource>();
            AudioSource gameSfxSource = GameObject.Find("Game SFX")?.GetComponent<AudioSource>();

            if (gameMusicSource != null && gameSfxSource != null)
            {
                AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);
                AudioManager.instance.PlayGameMusic();
            }
            else
            {
                Debug.LogError("Game audio sources not found.");
            }
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }
}
