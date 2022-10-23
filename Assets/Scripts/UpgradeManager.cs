using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager upgradeManager;

    float[] swapSpeedTiers = new float[]
    {
        1.0f, 0.9f, 0.75f, 0.6f, 0.45f, 0.3f
    };
    int[] swapSpeedCosts = new int[]
    {
        0, 100, 200, 400, 800, 1000
    };
    public int currentSwapTier;

    float[] jumpCooldownTiers = new float[]
    {
        3.0f, 2.7f, 2.4f, 2.1f, 1.8f, 1.5f
    };
    int[] jumpCooldownCosts = new int[]
    {
        0, 150, 300, 450, 600, 750
    };
    public int currentJumpCoolTier;

    float[] maxHealthTiers = new float[]
    {
        3, 4, 5, 6, 7, 8
    };
    int[] maxHealthCosts = new int[]
    {
        0, 200, 400, 800, 1200, 1600
    };
    public int currentMaxHealthTier;

    float[] jumpIFrameTiers = new float[]
    {
        0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f
    };
    int[] jumpIFrameCosts = new int[]
    {
        0, 200, 400, 600, 800, 1000
    };
    public int currentJumpIFrameTier;

    float[] currencyMultiplierTiers = new float[]
    {
        1.0f, 1.2f, 1.5f, 1.9f, 2.4f, 3.0f
    };
    int[] currencyMultiplierCosts = new int[]
    {
        0, 250, 400, 650, 800, 950
    };
    public int currentMultiplierTier;

    float[] glideTimeTiers = new float[]
    {
        0.0f, 0.5f, 1.0f, 1.5f, 2.0f, 2.5f
    };
    int[] glideTimeCosts = new int[]
    {
        0, 200, 400, 800, 1200, 1600
    };
    public int currentGlideTier;

    float[] wingsEnabledTiers = new float[]
    {
        0.0f, 1.0f
    };
    int[] wingsEnabledCosts = new int[]
    {
        0, 500
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
            //play sound to indicate insufficient money
            return;
        }

        if (swapSpeedCosts[currentSwapTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentSwapTier++;
            PlayerStats.playerStats.laneSwapSpeed = swapSpeedTiers[currentSwapTier];
            //play sound to indicate purchase has been made
        }
    }

    public void UpgradeJumpCooldown()
    {
        if (currentJumpCoolTier == jumpCooldownTiers.Length - 1) 
        {
            //play sound to indicate insufficient money
            return;
        }
        if (jumpCooldownCosts[currentJumpCoolTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentJumpCoolTier ++;
            PlayerStats.playerStats.jumpCooldown = jumpCooldownTiers[currentJumpCoolTier];
            //play sound to indicate purchase has been made
        }
    }

    public void UpgradeMaxHealth()
    {
        if (currentMaxHealthTier == maxHealthTiers.Length - 1)
        {
            //play sound to indicate insufficient money
            return;
        }
        if (maxHealthCosts[currentMaxHealthTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentMaxHealthTier++;
            PlayerStats.playerStats.maxHealth = (int)maxHealthTiers[currentMaxHealthTier];
            //play sound to indicate purchase has been made
        }
    }

    public void UpgradeJumpIFrames()
    {
        if (currentJumpIFrameTier == jumpIFrameTiers.Length - 1)
        {
            //play sound to indicate insufficient money
            return;
        }
        if (jumpIFrameCosts[currentJumpIFrameTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentJumpIFrameTier++;
            PlayerStats.playerStats.laneSwapSpeed = jumpIFrameTiers[currentJumpIFrameTier];
            //play sound to indicate purchase has been made
        }
    }

    public void UpgradeCurrencyMultiplier()
    {
        if (currentMultiplierTier == currencyMultiplierTiers.Length - 1)
        {
            //play sound to indicate insufficient money
            return;
        }
        if (currencyMultiplierCosts[currentMultiplierTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentMultiplierTier++;
            PlayerStats.playerStats.currencyMultiplier = currencyMultiplierTiers[currentMultiplierTier];
            //play sound to indicate purchase has been made
        }

    }

    public void UpgradeGlideTime()
    {
        if (currentGlideTier == glideTimeTiers.Length - 1)
        {
            //play sound to indicate insufficient money
            return;
        }
        if (glideTimeCosts[currentGlideTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentGlideTier++;
            PlayerStats.playerStats.glideTime = glideTimeTiers[currentGlideTier];
            //play sound to indicate purchase has been made
        }

    }

    public void UpgradeWings()
    {
        if (currentWingEnabledTier == wingsEnabledTiers.Length - 1)
        {
            //play sound to indicate insufficient money
            return;
        }
        if (wingsEnabledCosts[currentWingEnabledTier + 1] <= PlayerStats.playerStats.totalCurrency)
        {
            currentWingEnabledTier++;
            PlayerStats.playerStats.wingsEnabled = true;
            //play sound to indicate purchase has been made
        }

    }

    public void FinishShopping()
    {
        DataManager.dataManager.SavePlayerData();
    }
}
