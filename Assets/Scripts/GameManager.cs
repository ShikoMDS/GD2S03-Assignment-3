using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public int playerLives = 3;

    public Transform[] respawnPoints; // Array of respawn points for each stage
    private int currentStage = 0;

    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        if (AudioManager.instance != null)
        {
            Debug.Log("Setting audio sources and playing game music.");
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

        if (playerLives > 0)
        {
            // Reset player position to the current stage's respawn point
            player.transform.position = respawnPoints[currentStage].position;
        }
        else
        {
            GameOver();
        }
    }

    public void CompleteStage()
    {
        currentStage++;

        // Ensure currentStage doesn't exceed the respawnPoints array bounds
        if (currentStage >= respawnPoints.Length)
        {
            Debug.LogWarning("No more stages! Current stage index exceeds respawn points.");
            return;
        }

        // Move the camera to the next stage
        CameraController.instance.MoveToNextStage(currentStage);
    }

    private void GameOver()
    {
        // Stop all audio to prevent overlapping
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopAllAudio(); // Add this to AudioManager
        }

        // Load the Menu scene
        Debug.Log("Game Over! Returning to Menu.");
        SceneManager.LoadScene("Menu");

        // Reassign audio for the menu after the scene loads
        StartCoroutine(ReinitializeMenuAudio());
    }

    private IEnumerator ReinitializeMenuAudio()
    {
        // Wait for the new scene to load
        yield return new WaitForSeconds(0.1f);

        if (AudioManager.instance != null)
        {
            // Locate the menu's audio sources in the new scene
            AudioSource menuMusicSource = GameObject.Find("Music")?.GetComponent<AudioSource>();
            AudioSource menuSfxSource = GameObject.Find("SFX")?.GetComponent<AudioSource>();

            if (menuMusicSource == null || menuSfxSource == null)
            {
                Debug.LogError("Menu audio sources not found in the scene.");
                yield break;
            }

            // Set the audio sources for the menu
            AudioManager.instance.SetAudioSources(menuMusicSource, menuSfxSource);
            AudioManager.instance.PlayMenuMusic(); // Start playing the menu music
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }

    public IEnumerator ReinitializeGameAudio()
    {
        // Wait for the new scene to load
        yield return new WaitForSeconds(0.1f);

        if (AudioManager.instance != null)
        {
            // Locate game audio sources in the new scene
            AudioSource gameMusicSource = GameObject.Find("Game Music")?.GetComponent<AudioSource>();
            AudioSource gameSfxSource = GameObject.Find("Game SFX")?.GetComponent<AudioSource>();

            if (gameMusicSource == null || gameSfxSource == null)
            {
                Debug.LogError("Game audio sources not found in the scene. Ensure GameMusic and GameSFX GameObjects exist and are properly named.");
                yield break;
            }

            Debug.Log("Game audio sources found. Reassigning to AudioManager.");
            // Set the audio sources for the game
            AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);
            AudioManager.instance.PlayGameMusic(); // Start playing game music
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }
}
