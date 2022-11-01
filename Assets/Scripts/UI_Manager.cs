using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Manager : MonoBehaviour
{
    //gameplay UI elements
    public TMP_Text CurrencyText;
    public TMP_Text HealthText;
    public TMP_Text CompletionText;
    public TMP_Text jumpText;
    private float levelLength;
    public Slider healthSlider;
    public Slider completionSlider;
    public Slider jumpSlider;
    public float HealthFraction;

    //upgrade UI elements
    public TMP_Text totalCurrencyText;
    public TMP_Text jumpCooldownText;
    public TMP_Text currencyMultiplierText;
    public TMP_Text maxHealthText;
    public TMP_Text jumpIFramesText;
    public TMP_Text wingsText;
    public TMP_Text glideTimeText;
    public TMP_Text swapSpeedText;
    public Button glideTimeButton;
    public TMP_Text glideButtonText;
    public TMP_Text laneSwapButton;
    public TMP_Text jumpCooldownButton;
    public TMP_Text currencyMultiplierButton;
    public TMP_Text maxHealthButton;
    public TMP_Text jumpIFramesButton;
    public TMP_Text wingsButton;


    //result UI elements
    public TMP_Text currentLevelText;
    public TMP_Text currencyCollectedText;
    public TMP_Text distanceTravelledText;
    public TMP_Text timeElapsedText;


    //canvases
    public Canvas gameplayCanvas;
    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas optionsCanvas;
    public Canvas winCanvas;
    public Canvas upgradeCanvas;
    public Canvas mainMenuCanvas;
    public Canvas openingCutsceneCanvas;

    //loadOptions parent for main menu
    public GameObject loadOptionsParent;
    public Button buttonLevelOne;
    public Button buttonLevelTwo;
    public Button buttonLevelThree;
    public Button buttonGoBack;

    public bool loadOptionsOpen;

    //Options UI elements
    public Slider MusicVolumeSlider;
    public Slider FXVolumeSlider;

    //End cutscene elements
    public GameObject endingCutsceneParent;
    public Sprite[] endingCutSceneFrames;
    private int activeEndFrame;


    //Starting cutscene elements
    public GameObject startingCutsceneFrameParent;
    public Sprite[] startingCutsceneFrames;
    private int activeStartFrame;

    //Tweening elements
    public GameObject resultsFadeParent;
    public GameObject imageLevelComplete;
    public GameObject imageRunEnded;
    public Button runEndToUpgrade;

    public GameObject savingObjectParent;
    public GameObject savingImage;

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
        CurrencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        HealthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        CompletionText = GameObject.Find("CompletionText").GetComponent<TMP_Text>();
        jumpText = GameObject.Find("JumpText").GetComponent<TMP_Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        completionSlider = GameObject.Find("CompletionSlider").GetComponent<Slider>();
        jumpSlider = GameObject.Find("JumpSlider").GetComponent<Slider>();
        state = UI_State.mainMenu;
        glideTimeButton.interactable = false;
        CloseLoadMenu();
        activeEndFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (state == UI_State.gameplay)
        {
            GameplayUpdate();
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

        if (state == UI_State.upgrade)
        {
            UpgradeMenuUpdate();
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

        if (DataManager.dataManager.saving)
        {
            savingObjectParent.SetActive(true);
        }
        else
        {
            savingObjectParent.SetActive(false);
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
            jumpSlider.value = ((float)PlayerMovement.playerMovement.elapsedTime / PlayerStats.playerStats.jumpCooldown);
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

            case UI_State.results:
                Time.timeScale = 0;
                Gameover();
                UpdateResultsText();
                break;

            case UI_State.cutscene:
                Time.timeScale = 0;
                Cutscene();
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

    public void UpgradeMenuUpdate()
    {
        totalCurrencyText.text = "Scum coin (SC): " + PlayerStats.playerStats.totalCurrency.ToString();

        if (UpgradeManager.upgradeManager.currentJumpCoolTier == UpgradeManager.upgradeManager.jumpCooldownTiers.Length - 1)
        {
            jumpCooldownText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpCoolTier + 1) +
                " Current value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier] +
                " Next value (Seconds): MAXED OUT";
            jumpCooldownButton.text = "MAXED OUT";
        }
        else
        {
            jumpCooldownText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpCoolTier + 1) +
                " Current value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier] +
                " Next value (Seconds): " + UpgradeManager.upgradeManager.jumpCooldownTiers[UpgradeManager.upgradeManager.currentJumpCoolTier + 1];
            jumpCooldownButton.text = UpgradeManager.upgradeManager.jumpCooldownCosts[UpgradeManager.upgradeManager.currentJumpCoolTier + 1] + " SC";

        }

        if (UpgradeManager.upgradeManager.currentMultiplierTier == UpgradeManager.upgradeManager.currencyMultiplierTiers.Length - 1)
        {
            currencyMultiplierText.text = "Level: " + (UpgradeManager.upgradeManager.currentMultiplierTier + 1) +
                " Current Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier] +
                " Next Value: MAXED OUT";
            currencyMultiplierButton.text = "MAXED OUT";
        }
        else
        {
            currencyMultiplierText.text = "Level: " + (UpgradeManager.upgradeManager.currentMultiplierTier + 1) +
               " Current Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier] +
               " Next Value: " + UpgradeManager.upgradeManager.currencyMultiplierTiers[UpgradeManager.upgradeManager.currentMultiplierTier + 1];
            currencyMultiplierButton.text = UpgradeManager.upgradeManager.currencyMultiplierCosts[UpgradeManager.upgradeManager.currentMultiplierTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentMaxHealthTier == UpgradeManager.upgradeManager.maxHealthTiers.Length - 1)
        {
            maxHealthText.text = "Level: " + (UpgradeManager.upgradeManager.currentMaxHealthTier + 1) +
                " Current Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier] +
                " Next Value: MAXED OUT";
            maxHealthButton.text = "MAXED OUT";
        }
        else
        {
            maxHealthText.text = "Level: " + (UpgradeManager.upgradeManager.currentMaxHealthTier + 1) +
               " Current Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier] +
               " Next Value: " + UpgradeManager.upgradeManager.maxHealthTiers[UpgradeManager.upgradeManager.currentMaxHealthTier + 1];
            maxHealthButton.text = UpgradeManager.upgradeManager.maxHealthCosts[UpgradeManager.upgradeManager.currentMaxHealthTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentJumpIFrameTier == UpgradeManager.upgradeManager.jumpIFrameTiers.Length - 1)
        {
            jumpIFramesText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpIFrameTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier] +
                " Next Value (Seconds): MAXED OUT";
            jumpIFramesButton.text = "MAXED OUT";
        }
        else
        {
            jumpIFramesText.text = "Level: " + (UpgradeManager.upgradeManager.currentJumpIFrameTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier] +
                " Next Value (Seconds): " + UpgradeManager.upgradeManager.jumpIFrameTiers[UpgradeManager.upgradeManager.currentJumpIFrameTier + 1];
            jumpIFramesButton.text = UpgradeManager.upgradeManager.jumpIFrameCosts[UpgradeManager.upgradeManager.currentJumpIFrameTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentWingEnabledTier == UpgradeManager.upgradeManager.wingsEnabledTiers.Length - 1)
        {
            wingsText.text = "Wings Acquired: " + PlayerStats.playerStats.wingsEnabled;
            wingsButton.text = "MAXED OUT";
            glideTimeButton.interactable = true;
        }
        else
        {
            wingsText.text = "Wings Acquired: " + PlayerStats.playerStats.wingsEnabled;
            wingsButton.text = UpgradeManager.upgradeManager.wingsEnabledCosts[UpgradeManager.upgradeManager.currentWingEnabledTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentGlideTier == UpgradeManager.upgradeManager.glideTimeTiers.Length - 1)
        {
            glideTimeText.text = "Level: " + (UpgradeManager.upgradeManager.currentGlideTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier] +
                " Next Value (Seconds): MAXED OUT";
            glideButtonText.text = "MAXED OUT";
        }
        else
        {
            glideTimeText.text = "Level: " + (UpgradeManager.upgradeManager.currentGlideTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier] +
                " Next Value (Seconds): " + UpgradeManager.upgradeManager.glideTimeTiers[UpgradeManager.upgradeManager.currentGlideTier + 1];
            glideButtonText.text = UpgradeManager.upgradeManager.glideTimeCosts[UpgradeManager.upgradeManager.currentGlideTier + 1] + " SC";
        }

        if (UpgradeManager.upgradeManager.currentSwapTier == UpgradeManager.upgradeManager.swapSpeedTiers.Length - 1)
        {
            swapSpeedText.text = "Level: " + (UpgradeManager.upgradeManager.currentSwapTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier] +
                " Next Value (Seconds): MAXED OUT";
            laneSwapButton.text = "MAXED OUT";
        }
        else
        {
            swapSpeedText.text = "Level: " + (UpgradeManager.upgradeManager.currentSwapTier + 1) +
                " Current Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier] +
                " Next Value (Seconds): " + UpgradeManager.upgradeManager.swapSpeedTiers[UpgradeManager.upgradeManager.currentSwapTier + 1];
            laneSwapButton.text = UpgradeManager.upgradeManager.swapSpeedCosts[UpgradeManager.upgradeManager.currentSwapTier + 1] + " SC";
        }









    }

    public void OpenLoadMenu()
    {
        loadOptionsParent.SetActive(true);
        EnableLoadButtons();
        loadOptionsOpen = true;
    }

    public void CloseLoadMenu()
    {
        loadOptionsParent.SetActive(false);
        loadOptionsOpen = false;
    }

    public void EnableLoadButtons()
    {
        if (PlayerStats.playerStats.LevelOneComplete == true)
        {
            buttonLevelOne.interactable = true;
        }
        else
        {
            buttonLevelOne.interactable = false;
        }

        if (PlayerStats.playerStats.LevelTwoComplete == true)
        {
            buttonLevelTwo.interactable = true;
        }
        else
        {
            buttonLevelTwo.interactable = false;
        }

        if (PlayerStats.playerStats.LevelThreeComplete == true)
        {
            buttonLevelThree.interactable = true;
        }
        else
        {
            buttonLevelThree.interactable = false;
        }
    }

    public void UpdateResultsText()
    {
        currentLevelText.text = "Level " + LevelManager.levelManager.activeLevel;
        currencyCollectedText.text = "Currency Collected: " + PlayerStats.playerStats.currency + " x " + PlayerStats.playerStats.currencyMultiplier + " = " + (int)(PlayerStats.playerStats.currency * PlayerStats.playerStats.currencyMultiplier) + "\r\n Total Currency: " + PlayerStats.playerStats.totalCurrency;
        if (PlayerMovement.playerMovement == null) return;
        distanceTravelledText.text = "Distance Travelled: " + PlayerMovement.playerMovement.gameObject.transform.position.y + " m";
    }


    //Cutscene methods
    public void ShowOpeningCutscene()
    {
        state = UI_State.cutscene;
        activeStartFrame = 0;
        startingCutsceneFrameParent.GetComponent<Image>().sprite = startingCutsceneFrames[activeStartFrame];
    }

    public void NextEndFrame()
    {
        activeEndFrame++;

        if (activeStartFrame == startingCutsceneFrames.Length)
        {

        }
        else
        {
            endingCutsceneParent.GetComponent<Image>().sprite = endingCutSceneFrames[activeEndFrame];
        }
    }

    public void NextStartFrame()
    {
        activeStartFrame++;
        if (activeStartFrame >= startingCutsceneFrames.Length)
        {
            PlayerStats.playerStats.FirstRun = false;
            DataManager.dataManager.SavePlayerData();
            state = UI_State.mainMenu;
            openingCutsceneCanvas.gameObject.SetActive(false);
        }
        else
        {
            startingCutsceneFrameParent.GetComponent<Image>().sprite = startingCutsceneFrames[activeStartFrame];
        }
    }


    //Tweening methods
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
        Debug.Log("Tweening Fade");
        Tween fadeIn = rendererToFade.DOFade(1, (float)0.5);
        yield return fadeIn.WaitForCompletion();
        Debug.Log("Tween Complete!");
        runEndToUpgrade.gameObject.SetActive(true);
    }

//methods to toggle active canvas based on state
public void MainMenu()
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

    public void Gameplay()
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
        openingCutsceneCanvas.enabled = false;
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
        openingCutsceneCanvas.enabled = false;
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
        openingCutsceneCanvas.enabled = false;
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
        openingCutsceneCanvas.enabled = false;
    }

    public void Cutscene()
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
