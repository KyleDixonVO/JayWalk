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
    private GameObject Player;
    private float levelLength;
    public Slider healthSlider;
    public Slider completionSlider;
    public Slider jumpSlider;
    public float HealthFraction;
    // Start is called before the first frame update
    void Start()
    {
        CurrencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        HealthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        CompletionText = GameObject.Find("CompletionText").GetComponent<TMP_Text>();
        jumpText = GameObject.Find("JumpText").GetComponent<TMP_Text>();
        Player = GameObject.Find("Player");
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        completionSlider = GameObject.Find("CompletionSlider").GetComponent<Slider>();
        jumpSlider = GameObject.Find("JumpSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrencyText.text = "Scum Coin: " + Player.GetComponent<PlayerStats>().currency;
        HealthText.text = "Health: " + Player.GetComponent<PlayerStats>().health + "/" + Player.GetComponent<PlayerStats>().maxHealth;
        CompletionText.text = "Level Progress: ";
        jumpText.text = "Jump Charge: ";

        HealthFraction = ((float)Player.GetComponent<PlayerStats>().health / (float)Player.GetComponent<PlayerStats>().maxHealth);
        //Debug.Log(HealthFraction);
        healthSlider.value = HealthFraction;
        levelLength = GameObject.Find("LaneParent").GetComponent<LaneParent>().levelLength;
        completionSlider.value = ((float)Player.transform.position.y / (float)levelLength);

        if (Player.GetComponent<PlayerStats>().canJump)
        {
            jumpSlider.value = 1.0f;
        }
        else
        {
            jumpSlider.value = ((float)Player.GetComponent<PlayerMovement>().elapsedTime / 1);
        }
        
    }
}
