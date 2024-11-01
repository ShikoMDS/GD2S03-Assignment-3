using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip clickSound;

    public AudioSource musicSource;
    public AudioSource sfxSource;

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

        LoadSettings();
    }

    private void LoadSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSource.mute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxSource.mute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    public void PlayMenuMusic()
    {
        if (musicSource.clip != menuMusic)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusic)
        {
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
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

    public void ToggleMusicMute(bool isSoundOn)
    {
        musicSource.mute = !isSoundOn; // If isSoundOn is true, unmute; if false, mute
        PlayerPrefs.SetInt("MusicMuted", musicSource.mute ? 1 : 0);
    }

    public void ToggleSFXMute(bool isSoundOn)
    {
        sfxSource.mute = !isSoundOn; // If isSoundOn is true, unmute; if false, mute
        PlayerPrefs.SetInt("SFXMuted", sfxSource.mute ? 1 : 0);
    }
}
