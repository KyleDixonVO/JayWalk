using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public int activeLevel;
    public int numberOfLevels = 3;
    public bool finalLevelComplete;

    public GameObject levelOneParent;
    public GameObject levelTwoParent;
    public GameObject levelThreeParent;

    void Awake()
    {
        if (levelManager == null)
        {
            levelManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (levelManager != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        finalLevelComplete = false;
        activeLevel = 1;
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementLevel()
    {
        activeLevel++;
        if (activeLevel > numberOfLevels)
        {
            activeLevel = numberOfLevels;
            finalLevelComplete = true;
        }
    }

    public void LevelComplete()
    {
        if (activeLevel == 1 && PlayerStats.playerStats.LevelOneComplete == false)
        {
            PlayerStats.playerStats.LevelOneComplete = true;
        }
        else if (activeLevel == 2 && PlayerStats.playerStats.LevelTwoComplete == false)
        {
            PlayerStats.playerStats.LevelTwoComplete = true;
        }
        else if (activeLevel == 3 && PlayerStats.playerStats.LevelThreeComplete == false)
        {
            PlayerStats.playerStats.LevelThreeComplete = true;
        }

        IncrementLevel();
        LoadLevel();
    }

    public void LoadLevel()
    {
        switch (activeLevel) 
        {
            case 1:
                levelOneParent.SetActive(true);
                levelTwoParent.SetActive(false);
                levelThreeParent.SetActive(false);
                break;

            case 2:
                levelOneParent.SetActive(false);
                levelTwoParent.SetActive(true);
                levelThreeParent.SetActive(false);
                break;

            case 3:
                levelOneParent.SetActive(false);
                levelTwoParent.SetActive(false);
                levelThreeParent.SetActive(true);
                break;
        }

    }

    public void LoadLevel1()
    {
        activeLevel = 1;
        LoadLevel();
    }

    public void LoadLevel2()
    {
        activeLevel = 2;
        LoadLevel();
    }

    public void LoadLevel3()
    {
        activeLevel = 3;
        LoadLevel();
    }

}
