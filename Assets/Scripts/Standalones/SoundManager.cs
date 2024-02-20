using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip bgm;
    
    public AudioClip keySound;
    public AudioClip nextLevelSound;
    public AudioClip coinSound;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayMusic(bgm);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayKeySound()
    {
        PlaySFX(keySound);
    }

    public void PlayNextLevel()
    {
        PlaySFX(nextLevelSound);
    }

    public void PlayCoinSound()
    {
        PlaySFX(coinSound);
    }
}
