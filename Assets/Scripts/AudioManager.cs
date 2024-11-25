using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Initialize audio settings directly from PlayerPrefs
    public void InitializeAudioSettings()
    {
        // Load music volume and mute state from PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = musicVolume;
        musicSource.mute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        // Load SFX volume and mute state from PlayerPrefs
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        sfxSource.volume = sfxVolume;
        sfxSource.mute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    public void PlayMenuMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopAllAudio()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }

    // Allow external scripts to adjust music volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}