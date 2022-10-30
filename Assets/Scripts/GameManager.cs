using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameplayActive;
    public bool escapePressed = false;
    public bool gameWon = false;
    public bool wonLevel = false;
    public bool gameLoss = false;
    public int sceneIndex;

    //SoundManager soundManager = new SoundManager();
    public static GameManager gameManager;
    private Scene scene;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DataManager.dataManager.LoadGlobalData();
        DataManager.dataManager.LoadPlayerData();
        
    }

    // Update is called once per frame
    void Update()
    {
        DevDebugInputs();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePressed = !escapePressed;
        }

        scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        if (!PlayerStats.playerStats.isAlive)
        {
            gameLoss = true;
        }

        if (PlayerMovement.playerMovement == null) return;
        if (PlayerMovement.playerMovement.atEndOfLevel)
        {
            wonLevel = true;
        }

    }

    public void ResetRun()
    {
        PlayerStats.playerStats.Reset();
        PlayerMovement.playerMovement.Reset();
        LaneParent.laneParent.Reset();
        gameLoss = false;
        gameWon = false;
        wonLevel = false;
    }

    public void SwitchToGamePlay()
    {
        SceneManager.LoadScene(1);
        UI_Manager.ui_manager.SwitchGameplay();
        gameplayActive = true;
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene(0);
        gameplayActive = false;
        UI_Manager.ui_manager.SwitchMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DevDebugInputs()
    {
        if (gameplayActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LaneParent.laneParent.Reset();
                PlayerStats.playerStats.Reset();
                PlayerMovement.playerMovement.Reset();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                DataManager.dataManager.LoadPlayerData();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                DataManager.dataManager.SavePlayerData();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                PlayerStats.playerStats.health++;
                PlayerStats.playerStats.currency++;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                UI_Manager.ui_manager.SwitchUpgrade();
                Debug.Log("Upgrade Menu Active");
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                PlayerStats.playerStats.totalCurrency = 900000;
            }
        }
    }

    public void LoadLevelOne()
    {
        SwitchToGamePlay();
    }

    public void LoadLevelTwo()
    {
        SwitchToGamePlay();
    }

    public void LoadLevelThree()
    {
        SwitchToGamePlay();
    }

    public void FinishShopping()
    {
        DataManager.dataManager.SavePlayerData();
        UI_Manager.ui_manager.SwitchGameplay();
        if (gameLoss)
        {
            ResetRun();
        }
        else if (wonLevel)
        {
            LevelManager.levelManager.LevelComplete();
            ResetRun();
        }
    }
}
