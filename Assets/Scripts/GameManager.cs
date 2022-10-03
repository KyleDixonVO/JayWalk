using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameplayActive;
    //SoundManager soundManager = new SoundManager();
    LaneParent laneParent;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        laneParent = GameObject.Find("LaneParent").GetComponent<LaneParent>();
        player = GameObject.Find("Player");
        gameplayActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameplayActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                laneParent.Reset();
                player.GetComponent<PlayerStats>().Reset();
                player.GetComponent<PlayerMovement>().Reset();
            }
        }
    }
}
