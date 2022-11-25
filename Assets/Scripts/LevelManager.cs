using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public int activeLevel;
    private int numberOfLevels = 3;
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
            Destroy(gameObject);
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

    private void IncrementLevel()
    {
        // increases active level, if the last level is completed the active level is set back to one and final level is registered as complete
        activeLevel++;
        if (activeLevel > numberOfLevels)
        {
            activeLevel = 1;
            finalLevelComplete = true;
        }
    }

    public void LevelComplete()
    {
        //Unlocks associated level in level select after level completion
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
        //Hides and displays appropriate level backgrounds
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


    //Methods used to load directly into a level from level select
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
