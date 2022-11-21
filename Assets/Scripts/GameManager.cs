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
    public Camera mainCam;

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
        gameWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UI_Manager.ui_manager.state == UI_Manager.UI_State.gameplay)
        {
            SoundManager.soundManager.PlayGameplayMusic();
            Debug.Log("Playing Gameplay Music");
        }
        else if (UI_Manager.ui_manager.state == UI_Manager.UI_State.mainMenu)
        {
            SoundManager.soundManager.PlayMainMenuMusic();
            Debug.Log("Playing Main Menu Music");
        }
        else if (UI_Manager.ui_manager.state == UI_Manager.UI_State.paused)
        {
            SoundManager.soundManager.gameplayMusic.Pause();
        }
        else
        {
            SoundManager.soundManager.StopMusic();
        }

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

        if((gameLoss == true || wonLevel == true) && PlayerStats.playerStats.CurrencyTotaled == false)
        {
            PlayerStats.playerStats.UpdateTotalCurrency();
        }

        if (LevelManager.levelManager.finalLevelComplete)
        {
            gameWon = true;
        }
        else if (LevelManager.levelManager.finalLevelComplete == false && UI_Manager.ui_manager.state != UI_Manager.UI_State.win)
        {
            gameWon = false;
        }
    }

    public void ResetRun()
    {
        PlayerStats.playerStats.Reset();
        UI_Manager.ui_manager.ResetLerpList();

        gameLoss = false;
        gameWon = false;
        wonLevel = false;

        if (LaneParent.laneParent != null) 
        {
            LaneParent.laneParent.Reset();
        }

        if (PlayerMovement.playerMovement != null)
        {
            PlayerMovement.playerMovement.Reset();
        }
 
    }

    public void SwitchToGamePlay()
    {
        if (PlayerStats.playerStats.FirstRun)
        {
            UI_Manager.ui_manager.ShowOpeningCutscene();
        }
        else
        {
            if (gameplayActive) return;
            Debug.Log("Switching to game play");
            SceneManager.LoadScene(1);
            UI_Manager.ui_manager.SwitchGameplay();
            gameplayActive = true;
            ResetRun();
        }
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
                //LaneParent.laneParent.Reset();
                //PlayerStats.playerStats.Reset();
                //PlayerMovement.playerMovement.Reset();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //DataManager.dataManager.LoadPlayerData();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                //DataManager.dataManager.SavePlayerData();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                //PlayerStats.playerStats.health++;
                //PlayerStats.playerStats.currency++;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                //UI_Manager.ui_manager.SwitchUpgrade();
                //Debug.Log("Upgrade Menu Active");
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                //PlayerStats.playerStats.totalCurrency = 900000;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                //PlayerMovement.playerMovement.gameObject.transform.position = LaneParent.laneParent.finishLine.transform.position;
            }
        }
    }

    public void LoadLevelOne()
    {
        SwitchToGamePlay();
        LevelManager.levelManager.LoadLevel1();
    }

    public void LoadLevelTwo()
    {
        SwitchToGamePlay();
        LevelManager.levelManager.LoadLevel2();
    }

    public void LoadLevelThree()
    {
        SwitchToGamePlay();
        LevelManager.levelManager.LoadLevel3();
    }

    public void FinishShopping()
    {
        DataManager.dataManager.SavePlayerData();
        UI_Manager.ui_manager.SwitchGameplay();
        if (gameLoss)
        {
            Debug.Log("reset run");
            ResetRun();
            Debug.Log(DataManager.dataManager.saving);
        }
        else if (wonLevel)
        {
            LevelManager.levelManager.LevelComplete();
            Debug.Log("level complete");
            ResetRun();
            Debug.Log(DataManager.dataManager.saving);
        }
    }
}
