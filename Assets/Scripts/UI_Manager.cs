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
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateSwitch();
        
    }

    public void GameplayUpdate()
    {
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
                break;

            case UI_State.gameplay:
                break;

            case UI_State.paused:
                break;

            case UI_State.options:
                break;

            case UI_State.win:
                break;

            case UI_State.upgrade:
                break;

            case UI_State.gameOver:
                break;
        }

    }
}
