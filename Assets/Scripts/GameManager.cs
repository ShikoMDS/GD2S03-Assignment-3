using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerLives;
    public int maxLives = 3;
    public HealthUIManager healthUIManager; // Reference to the UI Health Manager

    public Transform[] respawnPoints; // Array of respawn points for each stage
    private int currentStage = 0;

    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    public GameObject winScreen; // Win screen UI
    public GameObject loseScreen; // Lose screen UI

    private void Start()
    {
        ResetLives();
        InitializeAudio();

        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
    }

    private void InitializeAudio()
    {
        if (AudioManager.instance != null)
        {
            // Assign audio sources for game music and SFX
            AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);

            // Play game music if not already playing
            if (!AudioManager.instance.IsGameMusicPlaying())
            {
                AudioManager.instance.PlayGameMusic();
            }
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
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
        // Stop all audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopAllAudio();
        }

        if (hasWon)
        {
            Debug.Log("Player has won!");
            if (winScreen != null) winScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Player has lost!");
            if (loseScreen != null) loseScreen.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game

        // Reset lives and stages
        playerLives = maxLives;
        currentStage = 0;

        // Reload game scene
        SceneManager.LoadScene("Game");
        StartCoroutine(ReinitializeGameAudio());
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Menu");

        // Reinitialize audio for menu
        StartCoroutine(ReinitializeMenuAudio());
    }

    private IEnumerator ReinitializeGameAudio()
    {
        yield return new WaitForSeconds(0.1f); // Wait for the scene to load

        if (AudioManager.instance != null)
        {
            AudioSource newGameMusicSource = GameObject.Find("Game Music")?.GetComponent<AudioSource>();
            AudioSource newGameSfxSource = GameObject.Find("Game SFX")?.GetComponent<AudioSource>();

            if (newGameMusicSource != null && newGameSfxSource != null)
            {
                AudioManager.instance.SetAudioSources(newGameMusicSource, newGameSfxSource);
                AudioManager.instance.PlayGameMusic();
            }
            else
            {
                Debug.LogError("Game audio sources not found.");
            }
        }
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
    }
}
