using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource moneyAudio;
    public AudioSource damageAudio;
    public AudioSource healAudio;
    public AudioSource jumpAudio;
    public AudioSource glideAudio;
    public AudioSource landingAudio;
    public AudioSource menuBackAudio;
    public AudioSource menuSelectionAudio;
    public AudioSource mainMenuMusic;
    public AudioSource gameplayMusic;
    public float musicSliderValue;
    public float soundEffectsValue;
    public static SoundManager soundManager;

    void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        moneyAudio.playOnAwake = false;
        damageAudio.playOnAwake = false;
        healAudio.playOnAwake = false;
        mainMenuMusic.playOnAwake = false;
        gameplayMusic.playOnAwake = false;

        moneyAudio.volume = soundEffectsValue;
        damageAudio.volume = soundEffectsValue;
        healAudio.volume = soundEffectsValue;
        mainMenuMusic.volume = musicSliderValue;
        gameplayMusic.volume = musicSliderValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHealAudio()
    {
        if (healAudio.isPlaying) return;
        healAudio.Play();
    }

    public void PlayDamageAudio()
    {
        if (damageAudio.isPlaying) return;
        damageAudio.Play();
    }

    public void PlayMoneyAudio()
    {
        if (moneyAudio.isPlaying) return;
        moneyAudio.Play();
    }

    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic.isPlaying) return;
        mainMenuMusic.Play();
    }

    public void PlayGameplayMusic()
    {
        if (gameplayMusic.isPlaying) return;
        gameplayMusic.Play();
    }
}
