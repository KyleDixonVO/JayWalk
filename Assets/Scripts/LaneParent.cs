
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneParent : MonoBehaviour
{
    private int numberOfLanes = 5;
    public GameObject[] lane;
    public GameObject[,]obstacleLists;
    public GameObject ObstaclePrefab;
    public GameObject CurrencyPrefab;
    public GameObject HealthUpPrefab;
    public float minObstacleSpacing = 10.0f;
    public float maxObstacleSpacing = 20.0f;
    private float _levelLength = 1000;
    public float levelLength;
    public float generationDistance;

    // Start is called before the first frame update
    void Start()
    {
        levelLength = _levelLength;
        lane = new GameObject[numberOfLanes];
        obstacleLists = new GameObject[lane.Length,1000];
        for (int i = 0; i < 5; i++)
        {
            lane[i] = this.gameObject.transform.GetChild(i).gameObject;
        }

        PopulateLanes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateLanes()
    {
        for (int i = 0; i < lane.Length; i++)
        {
            generationDistance = 0;

                for (int j = 0; j < _levelLength; j++)
                {
                    while (generationDistance < _levelLength)
                    {
                        //obstacleLists[i,j] = new GameObject();
                        int nextObject = UnityEngine.Random.Range(0, 20);
                        generationDistance += UnityEngine.Random.Range(minObstacleSpacing, maxObstacleSpacing);

                        if (nextObject >= 19)
                        {
                            GameObject temp = Instantiate(HealthUpPrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                        else if (nextObject > 10)
                        {
                            GameObject temp = Instantiate(CurrencyPrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                        else
                        {
                            GameObject temp = Instantiate(ObstaclePrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                    }

                }
            

        }
    }

    public void Reset()
    {
        for (int i = 0; i < lane.Length; i++)
        {
            for (int j = 0; j < lane[i].transform.childCount; j++)
            {
                Debug.Log(i + " " + j);
                Destroy(lane[i].transform.GetChild(j).gameObject);
            }
        }
        Debug.Log("Cleared Array");
        PopulateLanes();
    }
}
