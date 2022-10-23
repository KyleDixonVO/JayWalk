using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;
    public int currency;
    public int totalCurrency;
    public float invincibilityTime;
    public float laneSwapSpeed;
    public float jumpCooldown;
    public float jumpIFrames;
    public float currencyMultiplier;
    public float glideTime;

    public bool LevelOneComplete;
    public bool LevelTwoComplete;
    public bool LevelThreeComplete;
    public bool FirstRun;

    public bool isAlive;
    public bool wingsEnabled;
    public bool canJump;
    public bool isJumping;
    public bool invincible;

    public static PlayerStats playerStats;

    void Awake()
    {
        if (playerStats == null)
        {
            playerStats = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerStats != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        currency = 0;
        isAlive = true;
        canJump = true;
        jumpCooldown = 3.0f;
        invincible = false;
        isJumping = false;
        invincibilityTime = 0.5f;
        jumpIFrames = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            isAlive = false;
        }
    }

    public void Heal(int amountHealed)
    {
        if (amountHealed < 0) return;

        health += amountHealed;
        
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health <= 0)
        {
            health = 0;
            isAlive = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) return;

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            isAlive = false;
        }
    }

    public void IncreaseMoney(int amount)
    {
        if (amount < 0) return;

        currency += amount;

        if (currency < 0)
        {
            currency = 0;
        }
    }

    public void Reset()
    {
        health = maxHealth;
        currency = 0;
        isAlive = true;
        canJump = true;
    }
}
