using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static FollowCam followCam;
    private int offsetY = 5;
    private int offsetZ = 10;
    void Awake()
    {
        if (followCam == null)
        {
            followCam = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (followCam != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the player exists, and if it does, the camera follows the player
        if (PlayerMovement.playerMovement == null) return;
        gameObject.transform.position = new Vector3(0, PlayerMovement.playerMovement.transform.position.y + offsetY, -offsetZ);
    }
}
