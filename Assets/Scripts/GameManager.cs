using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameplayActive;
    //SoundManager soundManager = new SoundManager();
    public static GameManager gameManager;

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
        gameplayActive = true;
    }

    // Update is called once per frame
    void Update()
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
