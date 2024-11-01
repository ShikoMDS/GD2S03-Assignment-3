using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip clickSound; // Assign the click sound here

    public AudioSource musicSource;
    public AudioSource sfxSource; // Separate AudioSource for SFX

    private static AudioManager instance;

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

        // Load and set initial settings
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Set music and SFX volume from PlayerPrefs if available
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSource.mute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxSource.mute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayClickSound()
    {
        sfxSource.PlayOneShot(clickSound);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void ToggleMusicMute(bool isMuted)
    {
        musicSource.mute = isMuted;
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
    }

    public void ToggleSFXMute(bool isMuted)
    {
        sfxSource.mute = isMuted;
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);
    }
}
