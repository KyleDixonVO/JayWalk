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
    public AudioSource insufficientFundsAudio;
    public AudioSource purchaseAudio;
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
        DataManager.dataManager.LoadGlobalData();
        UI_Manager.ui_manager.MusicVolumeSlider.value = DataManager.dataManager.musicVolume;
        UI_Manager.ui_manager.FXVolumeSlider.value = DataManager.dataManager.FXVolume;
        Debug.Log(DataManager.dataManager.FXVolume + "  " + UI_Manager.ui_manager.FXVolumeSlider.value);

        moneyAudio.playOnAwake = false;
        damageAudio.playOnAwake = false;
        healAudio.playOnAwake = false;
        mainMenuMusic.playOnAwake = false;
        //gameplayMusic.playOnAwake = false;
        //insufficientFundsAudio.playOnAwake = false;
        //purchaseAudio.playOnAwake = false;

        UpdateSoundValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (UI_Manager.ui_manager.state != UI_Manager.UI_State.options) return;
        DataManager.dataManager.musicVolume = UI_Manager.ui_manager.MusicVolumeSlider.value;
        DataManager.dataManager.FXVolume = UI_Manager.ui_manager.FXVolumeSlider.value;
        
        UpdateSoundValues();
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

    public void PlayInsufficientFundsAudio()
    {
        if (insufficientFundsAudio.isPlaying) return;
        insufficientFundsAudio.Play();
    }

    public void PlayPurchaseAudio()
    {
        if (purchaseAudio.isPlaying) return;
        purchaseAudio.Play();
    }

    public void UpdateSoundValues()
    {
        moneyAudio.volume = DataManager.dataManager.FXVolume;
        damageAudio.volume = DataManager.dataManager.FXVolume;
        healAudio.volume = DataManager.dataManager.FXVolume;
        mainMenuMusic.volume = DataManager.dataManager.musicVolume;
        //gameplayMusic.volume = musicSliderValue;
        //insufficientFundsAudio.volume = soundEffectsValue;
        //purchaseAudio.volume = soundEffectsValue;
    }
}
