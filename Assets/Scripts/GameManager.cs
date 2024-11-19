using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public int playerLives = 3;
    public Transform spawnPoint; // Player respawn position

    private int currentStage = 0;

    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);
            AudioManager.instance.PlayGameMusic(); // Start game music
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistent across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        playerLives--;

        if (playerLives > 0)
        {
            // Reset player position to spawn point
            player.transform.position = spawnPoint.position;
        }
        else
        {
            GameOver();
        }
    }

    public void CompleteStage()
    {
        currentStage++;
        // Trigger camera to move to the next stage and update spawn point if needed
        CameraController.instance.MoveToNextStage(currentStage);
    }

    private void GameOver()
    {
        // Implement game-over logic or load GameOver scene
        SceneManager.LoadScene("Menu"); // Or display a Game Over screen
    }
}
