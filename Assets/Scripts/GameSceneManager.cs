using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public AudioSource gameMusicSource;
    public AudioSource gameSfxSource;

    private void Start()
    {
        Debug.Log("GameSceneManager started.");

        if (AudioManager.instance != null)
        {
            Debug.Log("Passing audio sources to AudioManager.");
            AudioManager.instance.SetAudioSources(gameMusicSource, gameSfxSource);
            AudioManager.instance.PlayGameMusic();
        }
        else
        {
            Debug.LogError("AudioManager instance is missing!");
        }
    }
}
