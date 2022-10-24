using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameplayActive;
    public bool escapePressed = false;
    public bool gameWon = false;
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
        }
    }
}
