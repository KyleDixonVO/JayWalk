using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static FollowCam followCam;
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
        this.gameObject.transform.position = new Vector3(0, PlayerMovement.playerMovement.transform.position.y + 5, -10);
    }
}
