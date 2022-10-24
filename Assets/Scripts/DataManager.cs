using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    public static DataManager dataManager;

    public float volume;

    void Awake()
    {
       if (dataManager == null)
       {
            dataManager = this;
            DontDestroyOnLoad(gameObject);
       }
       else if (dataManager != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveGlobalData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream globalFile = File.Create(Application.persistentDataPath + "/globalData.dat");

        GlobalData globalData = new GlobalData();
        globalData.volume = volume;


        binaryFormatter.Serialize(globalFile, globalData);
        globalFile.Close();
    }

    public void LoadGlobalData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream globalFile = File.Open(Application.persistentDataPath + "/globalData.dat", FileMode.Open);
            GlobalData globalData = (GlobalData)binaryFormatter.Deserialize(globalFile);
            globalFile.Close();

            globalData.volume = volume;

        }
    }

    public void ResetPlayerData()
    {
        PlayerStats.playerStats.maxHealth = 3;
        PlayerStats.playerStats.health = 3;
        PlayerStats.playerStats.currency = 0;
        PlayerStats.playerStats.invincibilityTime = 0.5f;
        PlayerStats.playerStats.laneSwapSpeed = 1.0f;
        PlayerStats.playerStats.jumpCooldown = 3.0f;
        PlayerStats.playerStats.jumpIFrames = 0.5f;
        PlayerStats.playerStats.wingsEnabled = false;
        PlayerStats.playerStats.currencyMultiplier = 1.0f;
        PlayerStats.playerStats.glideTime = 0.0f;
        PlayerStats.playerStats.LevelOneComplete = false;
        PlayerStats.playerStats.LevelTwoComplete = false;
        PlayerStats.playerStats.LevelThreeComplete = false;
        PlayerStats.playerStats.FirstRun = true;

        UpgradeManager.upgradeManager.currentGlideTier = 0;
        UpgradeManager.upgradeManager.currentJumpCoolTier = 0;
        UpgradeManager.upgradeManager.currentJumpIFrameTier = 0;
        UpgradeManager.upgradeManager.currentMaxHealthTier = 0;
        UpgradeManager.upgradeManager.currentMultiplierTier = 0;
        UpgradeManager.upgradeManager.currentSwapTier = 0;
        UpgradeManager.upgradeManager.currentWingEnabledTier = 0;
    }

    public void SavePlayerData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream playerFile = File.Create(Application.persistentDataPath + "/playerData.dat");

        PlayerData playerData = new PlayerData();

        playerData.maxHealth = PlayerStats.playerStats.maxHealth;
        playerData.health = PlayerStats.playerStats.health;
        playerData.currency = PlayerStats.playerStats.currency;
        playerData.invincibilityTime = PlayerStats.playerStats.invincibilityTime;
        playerData.laneSwapSpeed = PlayerStats.playerStats.laneSwapSpeed;
        playerData.jumpCooldown = PlayerStats.playerStats.jumpCooldown;
        playerData.jumpIFrames = PlayerStats.playerStats.jumpIFrames;
        playerData.wingsEnabled = PlayerStats.playerStats.wingsEnabled;
        playerData.currencyMultiplier = PlayerStats.playerStats.currencyMultiplier;
        playerData.LevelOneComplete = PlayerStats.playerStats.LevelOneComplete;
        playerData.LevelTwoComplete = PlayerStats.playerStats.LevelTwoComplete;
        playerData.LevelThreeComplete = PlayerStats.playerStats.LevelThreeComplete;
        playerData.FirstRun = PlayerStats.playerStats.FirstRun;
        playerData.totalCurrency = PlayerStats.playerStats.totalCurrency;
        playerData.glideTime = PlayerStats.playerStats.glideTime;

        playerData.currentGlideTier = UpgradeManager.upgradeManager.currentGlideTier;
        playerData.currentJumpCoolTier = UpgradeManager.upgradeManager.currentJumpCoolTier;
        playerData.currentJumpIFrameTier = UpgradeManager.upgradeManager.currentJumpIFrameTier;
        playerData.currentMaxHealthTier = UpgradeManager.upgradeManager.currentMaxHealthTier;
        playerData.currentMultiplierTier = UpgradeManager.upgradeManager.currentMultiplierTier;
        playerData.currentSwapTier = UpgradeManager.upgradeManager.currentSwapTier;
        playerData.currentWingEnabledTier = UpgradeManager.upgradeManager.currentWingEnabledTier;


        binaryFormatter.Serialize(playerFile, playerData);
        playerFile.Close();
    }

    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream playerFile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(playerFile);
            playerFile.Close();

            //set local playerData vars equal to loadedPlayerData

            PlayerStats.playerStats.maxHealth = playerData.maxHealth;
            PlayerStats.playerStats.health = playerData.health;
            PlayerStats.playerStats.currency = playerData.currency;
            PlayerStats.playerStats.invincibilityTime = playerData.invincibilityTime;
            PlayerStats.playerStats.laneSwapSpeed = playerData.laneSwapSpeed;
            PlayerStats.playerStats.jumpCooldown = playerData.jumpCooldown;
            PlayerStats.playerStats.jumpIFrames = playerData.jumpIFrames;
            PlayerStats.playerStats.wingsEnabled = playerData.wingsEnabled;
            PlayerStats.playerStats.currencyMultiplier = playerData.currencyMultiplier;
            PlayerStats.playerStats.LevelOneComplete = playerData.LevelOneComplete;
            PlayerStats.playerStats.LevelTwoComplete = playerData.LevelTwoComplete;
            PlayerStats.playerStats.LevelThreeComplete = playerData.LevelThreeComplete;
            PlayerStats.playerStats.FirstRun = playerData.FirstRun;
            PlayerStats.playerStats.totalCurrency = playerData.totalCurrency;
            PlayerStats.playerStats.glideTime = playerData.glideTime;

            UpgradeManager.upgradeManager.currentGlideTier = playerData.currentGlideTier;
            UpgradeManager.upgradeManager.currentJumpCoolTier = playerData.currentJumpCoolTier;
            UpgradeManager.upgradeManager.currentJumpIFrameTier = playerData.currentJumpIFrameTier;
            UpgradeManager.upgradeManager.currentMaxHealthTier = playerData.currentMaxHealthTier;
            UpgradeManager.upgradeManager.currentMultiplierTier = playerData.currentMultiplierTier;
            UpgradeManager.upgradeManager.currentSwapTier = playerData.currentSwapTier;
            UpgradeManager.upgradeManager.currentWingEnabledTier = playerData.currentWingEnabledTier;
        }
    }

    [Serializable]
    public class PlayerData 
    {
        public int maxHealth;
        public int health;
        public int currency;
        public int totalCurrency;
        public float invincibilityTime;
        public float laneSwapSpeed;
        public float jumpCooldown;
        public float jumpIFrames;
        public float currencyMultiplier;
        public float glideTime;
        public bool wingsEnabled;

        public bool LevelOneComplete;
        public bool LevelTwoComplete;
        public bool LevelThreeComplete;
        public bool FirstRun;

        public int currentSwapTier;
        public int currentJumpCoolTier;
        public int currentMaxHealthTier;
        public int currentJumpIFrameTier;
        public int currentMultiplierTier;
        public int currentGlideTier;
        public int currentWingEnabledTier;
    }

    [Serializable]

    public class GlobalData 
    {
        public float volume;
    }


}
