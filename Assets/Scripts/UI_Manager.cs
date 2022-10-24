using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text CurrencyText;
    public TMP_Text HealthText;
    public TMP_Text CompletionText;
    public TMP_Text jumpText;
    private float levelLength;
    public Slider healthSlider;
    public Slider completionSlider;
    public Slider jumpSlider;
    public float HealthFraction;

    public Canvas gameplayCanvas;
    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas optionsCanvas;
    public Canvas winCanvas;
    public Canvas upgradeCanvas;
    public Canvas mainMenuCanvas;


    public static UI_Manager ui_manager;

    public enum UI_State 
    { 
        gameplay,
        paused,
        mainMenu,
        options,
        win,
        upgrade,
        gameOver
    }

    public UI_State state;
    public UI_State returnFromPause;
    public UI_State returnFromOptions;


    void Awake()
    {
        if (ui_manager == null)
        {
            ui_manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ui_manager != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        HealthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        CompletionText = GameObject.Find("CompletionText").GetComponent<TMP_Text>();
        jumpText = GameObject.Find("JumpText").GetComponent<TMP_Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        completionSlider = GameObject.Find("CompletionSlider").GetComponent<Slider>();
        jumpSlider = GameObject.Find("JumpSlider").GetComponent<Slider>();
        state = UI_State.mainMenu;
    }

    // Update is called once per frame
    void Update()
    {

        if (state == UI_State.gameplay)
        {
            GameplayUpdate();
        }

        if (state == UI_State.gameplay && GameManager.gameManager.gameLoss == true)
        {
            state = UI_State.gameOver;
        }

        if (state == UI_State.gameplay && GameManager.gameManager.gameWon == true)
        {
            state = UI_State.win;
        }

        if (state == UI_State.gameplay && GameManager.gameManager.escapePressed)
        {
            returnFromPause = state;
            state = UI_State.paused;
        }
        else if (state == UI_State.paused && !GameManager.gameManager.escapePressed)
        {
            state = returnFromPause;
        }

        EvaluateSwitch();
    }

    public void GameplayUpdate()
    {
        if (PlayerMovement.playerMovement == null || PlayerStats.playerStats == null || LaneParent.laneParent == null) return;

        CurrencyText.text = "Scum Coin: " + PlayerStats.playerStats.currency;
        HealthText.text = "Health: " + PlayerStats.playerStats.health + "/" + PlayerStats.playerStats.maxHealth;
        CompletionText.text = "Level Progress: ";
        jumpText.text = "Jump Charge: ";

        HealthFraction = ((float)PlayerStats.playerStats.health / (float)PlayerStats.playerStats.maxHealth);
        //Debug.Log(HealthFraction);
        healthSlider.value = HealthFraction;
        levelLength = LaneParent.laneParent.levelLength;
        completionSlider.value = ((float)PlayerMovement.playerMovement.transform.position.y / (float)levelLength);

        if (PlayerStats.playerStats.canJump)
        {
            jumpSlider.value = 1.0f;
        }
        else
        {
            jumpSlider.value = ((float)PlayerMovement.playerMovement.elapsedTime / 1);
        }
    }

    public void EvaluateSwitch()
    {
        switch (state) 
        {
            case UI_State.mainMenu:
                MainMenu();
                Time.timeScale = 0;
                break;

            case UI_State.gameplay:
                Gameplay();
                Time.timeScale = 1;
                break;

            case UI_State.paused:
                Time.timeScale = 0;
                Pause();
                break;

            case UI_State.options:
                Time.timeScale = 0;
                Options();
                break;

            case UI_State.win:
                Time.timeScale = 0;
                Win();
                break;

            case UI_State.upgrade:
                Time.timeScale = 0;
                Upgrade();
                break;

            case UI_State.gameOver:
                Time.timeScale = 0;
                Gameover();
                break;
        }

    }

    public void ReturnFromOptions()
    {
        state = returnFromOptions;
    }

    public void ReturnFromPause()
    {
        GameManager.gameManager.escapePressed = false;
    }

    public void SwitchMainMenu()
    {
        state = UI_State.mainMenu;
    }

    public void SwitchOptions()
    {
        returnFromOptions = state;
        state = UI_State.options;
    }

    public void SwitchGameplay()
    {
        state = UI_State.gameplay;
    }

    public void MainMenu()
    {
        mainMenuCanvas.enabled = true;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void Gameplay()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = true;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void Pause()
    {
        pauseCanvas.enabled = true;
        gameplayCanvas.enabled = true;
        optionsCanvas.enabled = false;
    }

    public void Options()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = true;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void Win()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = true;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void Gameover()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    public void Upgrade()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }
}
