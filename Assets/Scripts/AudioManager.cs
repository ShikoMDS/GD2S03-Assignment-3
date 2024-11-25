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

   public static AudioManager instance;

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
            StopAllAudio();
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        Debug.Log("PlayGameMusic called!"); // Add this for debugging

        if (musicSource.clip != gameMusic)
        {
            StopAllAudio();
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
    public void InitializeAudioSources()
    {
        // Ensure the music source is assigned or created
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the SFX source is assigned or created
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log("Audio sources initialized or reassigned.");
    }

    public void SetAudioSources(AudioSource newMusicSource, AudioSource newSfxSource)
    {
        // Apply saved settings
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bool isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bool isSfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        
        musicSource = newMusicSource;
        sfxSource = newSfxSource;

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        musicSource.mute = isMusicMuted;
        sfxSource.mute = isSfxMuted;

        Debug.Log($"Audio sources updated: MusicVolume={musicVolume}, SFXVolume={sfxVolume}, MusicMuted={isMusicMuted}, SFXMuted={isSfxMuted}");
    }
    public void StopAllAudio()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }

        if (sfxSource != null)
        {
            sfxSource.Stop();
        }

        Debug.Log("All audio stopped.");
    }
    public bool IsGameMusicPlaying()
    {
        // Check if musicSource is playing
        return musicSource != null && musicSource.isPlaying;
    }
}
