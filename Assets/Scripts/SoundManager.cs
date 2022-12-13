using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource moneyAudio;
    public AudioSource damageAudio;
    public AudioSource healAudio;
    public AudioSource jumpAudio;
    public AudioSource landingAudio;
    public AudioSource menuBackAudio;
    public AudioSource menuSelectionAudio;
    public AudioSource mainMenuMusic;
    public AudioSource gameplayMusic;
    public AudioSource insufficientFundsAudio;
    public AudioSource purchaseAudio;
    public AudioSource gemAudio;
    public AudioSource jumpChargeAudio;
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
        UI_Manager.ui_manager.musicVolumeSlider.value = DataManager.dataManager.musicVolume;
        UI_Manager.ui_manager.fxVolumeSlider.value = DataManager.dataManager.FXVolume;

        TurnOffPlayOnAwake();

        UpdateSoundValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (UI_Manager.ui_manager.state != UI_Manager.UI_State.options) return;
        DataManager.dataManager.musicVolume = UI_Manager.ui_manager.musicVolumeSlider.value;
        DataManager.dataManager.FXVolume = UI_Manager.ui_manager.fxVolumeSlider.value;
        
        UpdateSoundValues();
    }

    // methods used to play individual sounds -------------------------------------------------------------------------------------------------------------------------------------------
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
        gameplayMusic.Stop();
    }

    public void PlayGameplayMusic()
    {
        if (gameplayMusic.isPlaying) return;
        gameplayMusic.Play();
        mainMenuMusic.Stop();
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

    public void PlayJumpAudio()
    {
        if (jumpAudio.isPlaying) return;
        jumpAudio.Play();
    }

    public void PlayLandingAudio()
    {
        if (landingAudio.isPlaying) return;
        landingAudio.Play();
    }

    public void PlayMenuBackAudio()
    {
        if (menuBackAudio.isPlaying) return;
        menuBackAudio.Play();
    }

    public void PlayMenuSelectionAudio()
    {
        if (menuSelectionAudio.isPlaying) return;
        menuSelectionAudio.Play();
    }

    public void PlayGemAudio()
    {
        if (gemAudio.isPlaying) return;
        gemAudio.Play();
    }

    public void PlayJumpChargeAudio()
    {
        if (jumpChargeAudio.isPlaying) return;
        jumpChargeAudio.Play();
    }

    public void StopMusic()
    {
        gameplayMusic.Stop();
        mainMenuMusic.Stop();
    }

    // methods used to group similar sound operations together --------------------------------------------------------------------------------------------------------------------------------
    private void UpdateSoundValues()
    {
        moneyAudio.volume = DataManager.dataManager.FXVolume;
        damageAudio.volume = DataManager.dataManager.FXVolume;
        healAudio.volume = DataManager.dataManager.FXVolume;
        mainMenuMusic.volume = DataManager.dataManager.musicVolume;
        gameplayMusic.volume = DataManager.dataManager.FXVolume;
        insufficientFundsAudio.volume = DataManager.dataManager.FXVolume;
        purchaseAudio.volume = DataManager.dataManager.FXVolume;
        jumpAudio.volume = DataManager.dataManager.FXVolume;
        landingAudio.volume = DataManager.dataManager.FXVolume;
        menuBackAudio.volume = DataManager.dataManager.FXVolume;
        menuSelectionAudio.volume = DataManager.dataManager.FXVolume;
        gemAudio.volume = DataManager.dataManager.FXVolume;
        jumpChargeAudio.volume = DataManager.dataManager.FXVolume;
    }

    private void TurnOffPlayOnAwake()
    {
        moneyAudio.playOnAwake = false;
        damageAudio.playOnAwake = false;
        healAudio.playOnAwake = false;
        mainMenuMusic.playOnAwake = false;
        gameplayMusic.playOnAwake = false;
        insufficientFundsAudio.playOnAwake = false;
        purchaseAudio.playOnAwake = false;
        jumpAudio.playOnAwake = false;
        landingAudio.playOnAwake = false;
        menuBackAudio.playOnAwake = false;
        menuSelectionAudio.playOnAwake = false;
        gemAudio.playOnAwake = false;
        jumpChargeAudio.playOnAwake = false;
    }
}
