using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_Manager : MonoBehaviour
{
    [Header("Gameplay UI Elements")]
    //gameplay UI elements
    public TMP_Text currencyText;
    public TMP_Text healthText;
    public TMP_Text completionText;
    public TMP_Text jumpText;
    private float levelLength;
    public Slider healthSlider;
    public Slider completionSlider;
    public Slider jumpSlider;
    public Slider glideSlider;
    public GameObject glideSliderParent;
    private float healthFraction;
    public GameObject coinLerpEndpoint;
    public List<GameObject> coinLerpList;

    private float lerpSpeed = 0.5f;
    private float distFromLerpTarget = 0.5f;

    [Header("Upgrade Screen UI Elements")]
    //upgrade UI elements
    public TMP_Text totalCurrencyText;
    public TMP_Text upgradePanelText;
    public Button glideTimeButton;
    public TMP_Text glideButtonText;
    public TMP_Text laneSwapButton;
    public TMP_Text jumpCooldownButton;
    public TMP_Text currencyMultiplierButton;
    public TMP_Text maxHealthButton;
    public TMP_Text jumpIFramesButton;
    public TMP_Text wingsButton;
    public GameObject PipParent;

    [Header("Result Screen UI Elements")]
    //result UI elements
    public TMP_Text currentLevelText;
    public TMP_Text currencyCollectedText;
    public TMP_Text distanceTravelledText;
    public TMP_Text timeElapsedText;

    [Header("Canvases")]
    //canvases
    public Canvas gameplayCanvas;
    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas optionsCanvas;
    public Canvas winCanvas;
    public Canvas upgradeCanvas;
    public Canvas mainMenuCanvas;
    public Canvas openingCutsceneCanvas;

    [Header("Load Menu Elements")]
    //loadOptions parent for main menu
    public GameObject loadOptionsParent;
    public Button buttonLevelOne;
    public Button buttonLevelTwo;
    public Button buttonLevelThree;
    public Button buttonGoBack;
    public Button buttonStartGame;
    public Button buttonLoadLevels;
    public Button buttonOptions;
    public Button buttonQuit;
    public GameObject lock1;
    public GameObject lock2;
    public GameObject lock3;

    public bool loadOptionsOpen;

    [Header("Options UI Elements")]
    //Options UI elements
    public Slider musicVolumeSlider;
    public Slider fxVolumeSlider;
    public Button buttonReset;
    public Button buttonCloseOptions;
    public Button buttonControls;
    public Button optionsToMain;
    public GameObject controlMenuParent;

    [Header("End Cutscene UI Elements")]
    //End cutscene elements
    public GameObject endingCutsceneParent;
    public Sprite[] endingCutSceneFrames;
    private int activeEndFrame;

    [Header("Starting Cutscene UI Elements")]
    //Starting cutscene elements
    public GameObject startingCutsceneFrameParent;
    public Sprite[] startingCutsceneFrames;
    private int activeStartFrame;

    [Header("Tweening Elements")]
    //Tweening elements
    public GameObject resultsFadeParent;
    public GameObject imageLevelComplete;
    public GameObject imageRunEnded;
    public Button buttonRunEndToUpgrade;

    public GameObject savingObjectParent;
    public GameObject savingImage;

    [Header("Pause Menu Elements")]
    public GameObject ResetConfirmationParent;

    [Header("Event Elements")]
    //Event elements
    public GameObject caller;

    public static UI_Manager ui_manager;
    public enum UI_State
    {
        gameplay,
        paused,
        mainMenu,
        options,
        win,
        upgrade,
        results,
        cutscene
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
        controlMenuParent.SetActive(false);
        ResetConfirmationParent.SetActive(false);
        coinLerpList = new List<GameObject>();
        currencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        completionText = GameObject.Find("CompletionText").GetComponent<TMP_Text>();
        jumpText = GameObject.Find("JumpText").GetComponent<TMP_Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        completionSlider = GameObject.Find("CompletionSlider").GetComponent<Slider>();
        jumpSlider = GameObject.Find("JumpSlider").GetComponent<Slider>();
        state = UI_State.mainMenu;
        glideTimeButton.interactable = false;
        CloseLoadMenu();
        activeEndFrame = 0;
        Color color = savingImage.GetComponent<Image>().color;
        color.a = 0;
        savingImage.GetComponent<Image>().color = color;
        PipParent = GameObject.Find("PipParent");
    }

    // Update is called once per frame
    void Update()
    {
        MultistateChecks();

        EvaluateSwitch();
    }

    private void FixedUpdate()
    {
        if (state == UI_State.gameplay)
        {
            LerpCurrency();
        }
    }

    private void EvaluateSwitch()
    {
        switch (state)
        {
            case UI_State.mainMenu:
                FocusMainMenuCanvas();
                Time.timeScale = 0;
                break;

            case UI_State.gameplay:
                FocusGameplayCanvas();
                if (coinLerpEndpoint == null)
                {
                    coinLerpEndpoint = GameObject.Find("coinLerpEndpoint");
                }
                GameplayUpdate();

                if (GameManager.gameManager.gameWon == true)
                {
                    state = UI_State.win;

                    if (activeEndFrame == 0)
                    {
                        endingCutsceneParent.GetComponent<Image>().sprite = endingCutSceneFrames[activeEndFrame];
                    }
                }

                Time.timeScale = 1;
                break;

            case UI_State.paused:
                Time.timeScale = 0;
                FocusPauseCanvas();
                break;

            case UI_State.options:
                Time.timeScale = 0;
                FocusOptionsCanvas();

                if(returnFromOptions == UI_State.paused)
                {
                    buttonReset.interactable = false;
                }
                else
                {
                    buttonReset.interactable = true;
                }
                break;

            case UI_State.win:
                Time.timeScale = 0;
                FocusWinCanvas();
                break;

            case UI_State.upgrade:
                Time.timeScale = 0;
                FocusUpgradeCanvas();
                UpgradeMenuUpdate();
                break;

            case UI_State.results:
                Time.timeScale = 0;
                activeEndFrame = 0;
                FocusGameoverCanvas();
                UpdateResultsText();
                break;

            case UI_State.cutscene:
                Time.timeScale = 0;
                FocusCutsceneCanvas();
                break;
        }

    }

    private void MultistateChecks()
    {
        if (state == UI_State.gameplay && GameManager.gameManager.escapePressed)
        {
            returnFromPause = state;
            state = UI_State.paused;
        }
        else if (state == UI_State.paused && !GameManager.gameManager.escapePressed)
        {
            state = returnFromPause;
        }

        if (state == UI_State.gameplay && (GameManager.gameManager.gameLoss == true || GameManager.gameManager.wonLevel == true))
        {
            if (resultsFadeParent.activeInHierarchy == true) return;
            resultsFadeParent.SetActive(true);
            FadeInResultsTween();
        }
        else
        {
            resultsFadeParent.SetActive(false);
        }
    }


    //State toggles to be used with buttons -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        GameManager.gameManager.escapePressed = false;
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

    public void SwitchUpgrade()
    {
        state = UI_State.upgrade;
    }

    public void SwitchResults()
    {
        state = UI_State.results;
    }

    public void OpenResetMenu()
    {
        ResetConfirmationParent.SetActive(true);
        buttonCloseOptions.interactable = false;
        optionsToMain.interactable = false;
        buttonControls.interactable = false;
        buttonReset.interactable = false;
    }

    public void CloseResetMenu()
    {
        ResetConfirmationParent.SetActive(false);
        buttonCloseOptions.interactable = true;
        optionsToMain.interactable = true;
        buttonControls.interactable = true;
        buttonReset.interactable = true;
    }

    //Methods to update UI elements related to a certain state ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Upgrade ------------------------------------------------------------------------------------------------------------------------
    private void UpgradeMenuUpdate()
    {
        totalCurrencyText.text = "Scum coin (SC): " + PlayerStats.playerStats.totalCurrency.ToString();

        if (UpgradeManager.upgradeManager.currentSwapTier == UpgradeManager.upgradeManager.swapSpeedTiers.Length - 1)
        {
            laneSwapButton.text = "MAXED OUT";
        }
        else
        {
            laneSwapButton.text = UpgradeManager.upgradeManager.swapSpeedCosts[UpgradeManager.upgradeManager.currentSwapTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentJumpCoolTier == UpgradeManager.upgradeManager.jumpCooldownTiers.Length - 1)
        {
            jumpCooldownButton.text = "MAXED OUT";
        }
        else
        {
            jumpCooldownButton.text = UpgradeManager.upgradeManager.jumpCooldownCosts[UpgradeManager.upgradeManager.currentJumpCoolTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentMultiplierTier == UpgradeManager.upgradeManager.currencyMultiplierTiers.Length - 1)
        {
            currencyMultiplierButton.text = "MAXED OUT";
        }
        else
        {
            currencyMultiplierButton.text = UpgradeManager.upgradeManager.currencyMultiplierCosts[UpgradeManager.upgradeManager.currentMultiplierTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentMaxHealthTier == UpgradeManager.upgradeManager.maxHealthTiers.Length - 1)
        {
            maxHealthButton.text = "MAXED OUT";
        }
        else
        {
            maxHealthButton.text = UpgradeManager.upgradeManager.maxHealthCosts[UpgradeManager.upgradeManager.currentMaxHealthTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentJumpIFrameTier == UpgradeManager.upgradeManager.jumpIFrameTiers.Length - 1)
        {
            jumpIFramesButton.text = "MAXED OUT";
        }
        else
        {
            jumpIFramesButton.text = UpgradeManager.upgradeManager.jumpIFrameCosts[UpgradeManager.upgradeManager.currentJumpIFrameTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentWingEnabledTier == UpgradeManager.upgradeManager.wingsEnabledTiers.Length - 1)
        {
            wingsButton.text = "MAXED OUT";
            glideTimeButton.interactable = true;
        }
        else
        {
            wingsButton.text = UpgradeManager.upgradeManager.wingsEnabledCosts[UpgradeManager.upgradeManager.currentWingEnabledTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentGlideTier == UpgradeManager.upgradeManager.glideTimeTiers.Length - 1)
        {
            glideButtonText.text = "MAXED OUT";
        }
        else
        {
            glideButtonText.text = UpgradeManager.upgradeManager.glideTimeCosts[UpgradeManager.upgradeManager.currentGlideTier + 1] + " SC";
        }

        UpdatePips();

        if (caller == null) return;
        switch (caller.name)
        {
            case null:
                upgradePanelText.text = "No upgrade selected";
                break;

            case "button - LaneSwapSpeed":
                if (UpgradeManager.upgradeManager.currentSwapTier == UpgradeManager.upgradeManager.swapSpeedTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentSwapTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier] +
                        "\r\n Next Value (Seconds): MAXED OUT";
                    laneSwapButton.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentSwapTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier] +
                        "\r\n Next Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier + 1];
                    laneSwapButton.text = UpgradeManager.upgradeManager.swapSpeedCosts[UpgradeManager.upgradeManager.currentSwapTier + 1] + " SC";
                }
                break;

            case "button - JumpCooldown":
                if (UpgradeManager.upgradeManager.currentJumpCoolTier == UpgradeManager.upgradeManager.jumpCooldownTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpCoolTier + 1) +
                        "\r\n Current value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier] +
                        "\r\n Next value (Seconds): MAXED OUT";
                    jumpCooldownButton.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpCoolTier + 1) +
                        "\r\n Current value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier] +
                        "\r\n Next value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier + 1];
                    jumpCooldownButton.text = UpgradeManager.upgradeManager.jumpCooldownCosts[UpgradeManager.upgradeManager.currentJumpCoolTier + 1] + " SC";

                }
                break;

            case "button - CurrencyMultiplier":
                if (UpgradeManager.upgradeManager.currentMultiplierTier == UpgradeManager.upgradeManager.currencyMultiplierTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentMultiplierTier + 1) +
                        "\r\n Current Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier] +
                        "\r\n Next Value: MAXED OUT";
                    currencyMultiplierButton.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentMultiplierTier + 1) +
                       "\r\n Current Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier] +
                       "\r\n Next Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier + 1];
                    currencyMultiplierButton.text = UpgradeManager.upgradeManager.currencyMultiplierCosts[UpgradeManager.upgradeManager.currentMultiplierTier + 1] + " SC";
                }
                break;

            case "button - MaxHealth":
                if (UpgradeManager.upgradeManager.currentMaxHealthTier == UpgradeManager.upgradeManager.maxHealthTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentMaxHealthTier + 1) +
                        "\r\n Current Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier] +
                        "\r\n Next Value: MAXED OUT";
                    maxHealthButton.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentMaxHealthTier + 1) +
                       "\r\n Current Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier] +
                       "\r\n Next Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier + 1];
                    maxHealthButton.text = UpgradeManager.upgradeManager.maxHealthCosts[UpgradeManager.upgradeManager.currentMaxHealthTier + 1] + " SC";
                }
                break;

            case "button - JumpIFrames":
                if (UpgradeManager.upgradeManager.currentJumpIFrameTier == UpgradeManager.upgradeManager.jumpIFrameTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpIFrameTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier] +
                        "\r\n Next Value (Seconds): MAXED OUT";
                    jumpIFramesButton.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpIFrameTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier] +
                        "\r\n Next Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier + 1];
                    jumpIFramesButton.text = UpgradeManager.upgradeManager.jumpIFrameCosts[UpgradeManager.upgradeManager.currentJumpIFrameTier + 1] + " SC";
                }
                break;

            case "button - Wings":
                if (UpgradeManager.upgradeManager.currentWingEnabledTier == UpgradeManager.upgradeManager.wingsEnabledTiers.Length - 1)
                {
                    upgradePanelText.text = "Wings Acquired: " + PlayerStats.playerStats.wingsEnabled;
                    wingsButton.text = "MAXED OUT";
                    glideTimeButton.interactable = true;
                }
                else
                {
                    upgradePanelText.text = "Wings Acquired: " + PlayerStats.playerStats.wingsEnabled;
                    wingsButton.text = UpgradeManager.upgradeManager.wingsEnabledCosts[UpgradeManager.upgradeManager.currentWingEnabledTier + 1] + " SC";
                }
                break;

            case "button - GlideTime":
                if (UpgradeManager.upgradeManager.currentGlideTier == UpgradeManager.upgradeManager.glideTimeTiers.Length - 1)
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentGlideTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier] +
                        "\r\n Next Value (Seconds): MAXED OUT";
                    glideButtonText.text = "MAXED OUT";
                }
                else
                {
                    upgradePanelText.text = "Level: " + (UpgradeManager.upgradeManager.currentGlideTier + 1) +
                        "\r\n Current Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier] +
                        "\r\n Next Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier + 1];
                    glideButtonText.text = UpgradeManager.upgradeManager.glideTimeCosts[UpgradeManager.upgradeManager.currentGlideTier + 1] + " SC";
                }
                break;
        }

    }
    private void UpdatePips()
    {
        for (int i = 0; i < PipParent.transform.childCount; i++)
        {
            for (int j = 0; j < PipParent.transform.GetChild(i).transform.childCount; j++)
            {
                if (i == 0)
                {
                    if (UpgradeManager.upgradeManager.currentJumpCoolTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 1)
                {
                    if (UpgradeManager.upgradeManager.currentMultiplierTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 2)
                {
                    if (UpgradeManager.upgradeManager.currentMaxHealthTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 3)
                {
                    if (UpgradeManager.upgradeManager.currentJumpIFrameTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 4)
                {
                    if (UpgradeManager.upgradeManager.currentWingEnabledTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 5)
                {
                    if (UpgradeManager.upgradeManager.currentGlideTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
                else if (i == 6)
                {
                    if (UpgradeManager.upgradeManager.currentSwapTier >= j)
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        PipParent.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }
    }

    //Gameplay -----------------------------------------------------------------------------------------------------------------------
    private void GameplayUpdate()
    {
        if (PlayerMovement.playerMovement == null || PlayerStats.playerStats == null || LaneParent.laneParent == null) return;

        currencyText.text = "Scum Coin: " + PlayerStats.playerStats.currency;
        healthText.text = "Health: " + PlayerStats.playerStats.health + "/" + PlayerStats.playerStats.maxHealth;
        completionText.text = "Level Progress: ";
        jumpText.text = "Jump Charge";

        healthFraction = ((float)PlayerStats.playerStats.health / (float)PlayerStats.playerStats.maxHealth);
        healthSlider.value = healthFraction;
        levelLength = LaneParent.laneParent.levelLength;
        completionSlider.value = ((float)PlayerMovement.playerMovement.transform.position.y / (float)levelLength);

        if (!PlayerStats.playerStats.wingsEnabled || UpgradeManager.upgradeManager.currentGlideTier == 0)
        {
            glideSliderParent.SetActive(false);
        }
        else if(PlayerMovement.playerMovement != null)
        {
            glideSliderParent.SetActive(true);
            if (PlayerMovement.playerMovement.elapsedGlideTime == 0)
            {
                glideSlider.value = glideSlider.maxValue;
            }
            else
            {
                glideSlider.value = 1- ((float)PlayerMovement.playerMovement.elapsedGlideTime / PlayerStats.playerStats.glideTime);
            }
        }

        if (PlayerStats.playerStats.canJump)
        {
            jumpSlider.value = jumpSlider.maxValue;
        }
        else
        {
            jumpSlider.value = ((float)PlayerMovement.playerMovement.elapsedTime / PlayerStats.playerStats.jumpCooldown);
        }
    }

    //Results ------------------------------------------------------------------------------------------------------------------------
    private void UpdateResultsText()
    {
        currentLevelText.text = LevelManager.levelManager.activeLevel.ToString();
        currencyCollectedText.text = PlayerStats.playerStats.currency + " x " + PlayerStats.playerStats.currencyMultiplier + " = " + (int)(PlayerStats.playerStats.currency * PlayerStats.playerStats.currencyMultiplier) + " sc" + "\r\n" + PlayerStats.playerStats.totalCurrency + " sc";
        if (PlayerMovement.playerMovement == null) return;
        distanceTravelledText.text = PlayerMovement.playerMovement.gameObject.transform.position.y + " m";
    }

    //Main Menu ----------------------------------------------------------------------------------------------------------------------
    public void OpenLoadMenu()
    {
        loadOptionsParent.SetActive(true);
        EnableLoadButtons();
        loadOptionsOpen = true;
        buttonStartGame.interactable = false;
        buttonQuit.interactable = false;
        buttonOptions.interactable = false;
        buttonLoadLevels.interactable = false;
    }

    public void CloseLoadMenu()
    {
        loadOptionsParent.SetActive(false);
        loadOptionsOpen = false;
        buttonStartGame.interactable = true;
        buttonQuit.interactable = true;
        buttonOptions.interactable = true;
        buttonLoadLevels.interactable = true;
    }

    public void EnableLoadButtons()
    {
        if (PlayerStats.playerStats.LevelOneComplete == true)
        {
            buttonLevelOne.interactable = true;
            lock1.SetActive(false);
        }
        else
        {
            buttonLevelOne.interactable = false;
            lock1.SetActive(true);
        }

        if (PlayerStats.playerStats.LevelTwoComplete == true)
        {
            buttonLevelTwo.interactable = true;
            lock2.SetActive(false);
        }
        else
        {
            buttonLevelTwo.interactable = false;
            lock2.SetActive(true);
        }

        if (PlayerStats.playerStats.LevelThreeComplete == true)
        {
            buttonLevelThree.interactable = true;
            lock3.SetActive(false);
        }
        else
        {
            buttonLevelThree.interactable = false;
            lock3.SetActive(true);
        }
    }

    //Options ------------------------------------------------------------------------------------------------------------------------
    public void OpenControlsMenu()
    {
        controlMenuParent.SetActive(true);
    }

    public void CloseControlsMenu()
    {
        controlMenuParent.SetActive(false);
    }


    //Lerping Coins During Gameplay -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void LerpCurrency()
    {
        foreach (GameObject currency in coinLerpList)
        {
            currency.transform.position = Vector2.MoveTowards(currency.transform.position, coinLerpEndpoint.transform.position, lerpSpeed);
            if (Vector2.Distance(currency.transform.position, coinLerpEndpoint.transform.position) < distFromLerpTarget)
            {
                currency.SetActive(false);
            }
        }
    }

    public void AddToLerpList(GameObject gameObject)
    {
        coinLerpList.Add(gameObject);
    }

    public void ResetLerpList()
    {
        coinLerpList = new List<GameObject>();
    }


    //Cutscene methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ShowOpeningCutscene()
    {
        openingCutsceneCanvas.gameObject.SetActive(true);
        Debug.Log("Showing opening cutscene");
        state = UI_State.cutscene;
        activeStartFrame = 0;
        startingCutsceneFrameParent.GetComponent<Image>().sprite = startingCutsceneFrames[activeStartFrame];
    }

    public void NextEndFrame()
    {
        if (activeEndFrame < endingCutSceneFrames.Length - 1)
        {
            activeEndFrame++;
            endingCutsceneParent.GetComponent<Image>().sprite = endingCutSceneFrames[activeEndFrame];
        }
        else
        {
            LevelManager.levelManager.finalLevelComplete = false;
            return;
        }
    }

    public void NextStartFrame()
    {
        activeStartFrame++;
        if (activeStartFrame >= startingCutsceneFrames.Length)
        {
            PlayerStats.playerStats.FirstRun = false;
            DataManager.dataManager.SavePlayerData();
            state = UI_State.gameplay;
            GameManager.gameManager.SwitchToGamePlay();
            openingCutsceneCanvas.gameObject.SetActive(false);
        }
        else
        {
            startingCutsceneFrameParent.GetComponent<Image>().sprite = startingCutsceneFrames[activeStartFrame];
        }
    }


    //Tweening methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void FadeInResultsTween()
    {
        if (GameManager.gameManager.wonLevel)
        {
            imageLevelComplete.SetActive(true);
            imageRunEnded.SetActive(false);
            Color temp = imageLevelComplete.GetComponent<Image>().color;
            temp.a = 0;
            imageLevelComplete.GetComponent<Image>().color = temp;
            StartCoroutine(TweenFade(imageLevelComplete.GetComponent<Image>()));
        }
        else if (GameManager.gameManager.gameLoss)
        {
            imageLevelComplete.SetActive(false);
            imageRunEnded.SetActive(true);
            Color temp = imageRunEnded.GetComponent<Image>().color;
            temp.a = 0;
            imageRunEnded.GetComponent<Image>().color = temp;
            StartCoroutine(TweenFade(imageRunEnded.GetComponent<Image>()));
        }
    }

    IEnumerator TweenFade(Image rendererToFade)
    {
        Tween fadeIn = rendererToFade.DOFade(1, (float)0.5);
        yield return fadeIn.WaitForCompletion();
        buttonRunEndToUpgrade.gameObject.SetActive(true);
    }

    public void ShowSaveIndicator()
    {
        StartCoroutine(SaveImageFade());
    }

    IEnumerator SaveImageFade()
    {
        savingImage.GetComponent<Image>().DOFade(1, 1f).SetLoops(2, LoopType.Yoyo);
        yield return null;
    }


    //methods to toggle active canvas based on state -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void FocusMainMenuCanvas()
    {
        mainMenuCanvas.enabled = true;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusGameplayCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = true;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusPauseCanvas()
    {
        pauseCanvas.enabled = true;
        gameplayCanvas.enabled = true;
        optionsCanvas.enabled = false;
        
    }

    private void FocusOptionsCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = true;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusWinCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = true;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusGameoverCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusUpgradeCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = true;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = false;
    }

    private void FocusCutsceneCanvas()
    {
        mainMenuCanvas.enabled = false;
        gameplayCanvas.enabled = false;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        winCanvas.enabled = false;
        upgradeCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        openingCutsceneCanvas.enabled = true;
        
    }
}
