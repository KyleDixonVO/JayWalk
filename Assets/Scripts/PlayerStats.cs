using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;
    public int currency;
    public bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        currency = 0;
        isAlive = true;
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
    }
}
