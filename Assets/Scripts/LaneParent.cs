using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneParent : MonoBehaviour
{
    public int numberOfLanes = 5;
    public GameObject[] lane;
    private float minObstacleSpacing = 5.0f;
    private float maxObstacleSpacing = 15.0f;
    private float levelLength = 1000;

    // Start is called before the first frame update
    void Start()
    {
        lane = new GameObject[numberOfLanes];
        for (int i = 0; i < 5; i++)
        {
            lane[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateHazards()
    {
        //place obstacles along each lane based on the spacing of previous obstacles
    }
}
