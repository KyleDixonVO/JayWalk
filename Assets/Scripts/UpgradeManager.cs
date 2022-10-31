using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager upgradeManager;

    public float[] swapSpeedTiers = new float[]
    {
        1.0f, 2f, 4f, 8f, 16f, 32f
    };
    public int[] swapSpeedCosts = new int[]
    {
        0, 20, 40, 60, 80, 100
    };
    public int currentSwapTier;

    public float[] jumpCooldownTiers = new float[]
    {
        3.0f, 2.7f, 2.4f, 2.1f, 1.8f, 1.5f
    };
    public int[] jumpCooldownCosts = new int[]
    {
        0, 30, 45, 60, 75, 90
    };
    public int currentJumpCoolTier;

    public float[] maxHealthTiers = new float[]
    {
        3, 4, 5, 6, 7, 8
    };
    public int[] maxHealthCosts = new int[]
    {
        0, 25, 50, 75, 100, 125
    };
    public int currentMaxHealthTier;

    public float[] jumpIFrameTiers = new float[]
    {
        0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f
    };
    public int[] jumpIFrameCosts = new int[]
    {
        0, 20, 40, 60, 80, 100
    };
    public int currentJumpIFrameTier;

    public float[] currencyMultiplierTiers = new float[]
    {
        1.0f, 1.2f, 1.5f, 1.9f, 2.4f, 3.0f
    };
    public int[] currencyMultiplierCosts = new int[]
    {
        0, 30, 50, 70, 90, 110
    };
    public int currentMultiplierTier;

    public float[] glideTimeTiers = new float[]
    {
        0.0f, 0.5f, 1.0f, 1.5f, 2.0f, 2.5f
    };
    public int[] glideTimeCosts = new int[]
    {
        0, 25, 50, 75, 100, 125
    };
    public int currentGlideTier;

    public float[] wingsEnabledTiers = new float[]
    {
        0.0f, 1.0f
    };
    public int[] wingsEnabledCosts = new int[]
    {
        0, 50
    };
    public int currentWingEnabledTier;


    void Awake()
    {
        if (upgradeManager == null)
        {
            upgradeManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (upgradeManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpgradeSwapSpeed()
    {
        if (currentSwapTier == swapSpeedTiers.Length - 1)
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }

        if (swapSpeedCosts[currentSwapTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentSwapTier++;
            PlayerStats.playerStats.laneSwapSpeed = swapSpeedTiers[currentSwapTier];
            PlayerStats.playerStats.totalCurrency -= swapSpeedCosts[currentSwapTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.laneSwapSpeed);
        }
    }

    public void UpgradeJumpCooldown()
    {
        if (currentJumpCoolTier == jumpCooldownTiers.Length - 1) 
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }
        if (jumpCooldownCosts[currentJumpCoolTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentJumpCoolTier ++;
            PlayerStats.playerStats.jumpCooldown = jumpCooldownTiers[currentJumpCoolTier];
            PlayerStats.playerStats.totalCurrency -= jumpCooldownCosts[currentJumpCoolTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.jumpCooldown);
        }
    }

    public void UpgradeMaxHealth()
    {
        if (currentMaxHealthTier == maxHealthTiers.Length - 1)
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }
        if (maxHealthCosts[currentMaxHealthTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentMaxHealthTier++;
            PlayerStats.playerStats.maxHealth = (int)maxHealthTiers[currentMaxHealthTier];
            PlayerStats.playerStats.totalCurrency -= maxHealthCosts[currentMaxHealthTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.maxHealth);
        }
    }

    public void UpgradeJumpIFrames()
    {
        if (currentJumpIFrameTier == jumpIFrameTiers.Length - 1)
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }
        if (jumpIFrameCosts[currentJumpIFrameTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentJumpIFrameTier++;
            PlayerStats.playerStats.jumpIFrames = jumpIFrameTiers[currentJumpIFrameTier];
            PlayerStats.playerStats.totalCurrency -= jumpIFrameCosts[currentJumpIFrameTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.jumpIFrames);
        }
    }

    public void UpgradeCurrencyMultiplier()
    {
        if (currentMultiplierTier == currencyMultiplierTiers.Length - 1)
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }
        if (currencyMultiplierCosts[currentMultiplierTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentMultiplierTier++;
            PlayerStats.playerStats.currencyMultiplier = currencyMultiplierTiers[currentMultiplierTier];
            PlayerStats.playerStats.totalCurrency -= currencyMultiplierCosts[currentMultiplierTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.currencyMultiplier);
        }

    }

    public void UpgradeGlideTime()
    {
        if (currentGlideTier == glideTimeTiers.Length - 1)
        {

            SoundManager.soundManager.PlayInsufficientFundsAudio();
            return;
        }
        if (glideTimeCosts[currentGlideTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentGlideTier++;
            PlayerStats.playerStats.glideTime = glideTimeTiers[currentGlideTier];
            PlayerStats.playerStats.totalCurrency -= glideTimeCosts[currentGlideTier];
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.glideTime);
        }

    }

    public void UpgradeWings()
    {
        if (currentWingEnabledTier == wingsEnabledTiers.Length - 1)
        {
            SoundManager.soundManager.PlayInsufficientFundsAudio();
            UI_Manager.ui_manager.glideTimeButton.interactable = true;
            return;
        }
        if (wingsEnabledCosts[currentWingEnabledTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            Debug.Log("Attempting to buy wings");
            currentWingEnabledTier++;
            PlayerStats.playerStats.wingsEnabled = true;
            PlayerStats.playerStats.totalCurrency -= wingsEnabledCosts[currentWingEnabledTier];
            Debug.Log("Acquired Wings: " + PlayerStats.playerStats.wingsEnabled);
            SoundManager.soundManager.PlayPurchaseAudio();
            Debug.Log(PlayerStats.playerStats.wingsEnabled);
        }

    }
}
