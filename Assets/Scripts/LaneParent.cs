
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneParent : MonoBehaviour
{
    private int numberOfLanes = 5;
    public GameObject[] lane;
    public GameObject[,]obstacleLists;
    public GameObject ObstaclePrefab;
    public GameObject TallObstaclePrefab;
    public GameObject CurrencyPrefab;
    public GameObject HealthUpPrefab;
    public GameObject finishLine;
    public float minObstacleSpacing = 8.0f;
    public float maxObstacleSpacing = 20.0f;
    public int rngMax = 20;
    public int rngMin = 0;
    private float _levelLength;
    private float defaultLength = 1000;
    private float defaultMinObstacleSpacing = 8.0f;
    private float defaultMaxObstacleSpacing = 20.0f;
    private int defaultRngMax = 20;
    public float levelLength
    {
        get { return _levelLength; }
    }
    public float generationDistance;
    public static LaneParent laneParent;

    void Awake()
    {
        if (laneParent == null)
        {
            laneParent = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (laneParent != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGenerationPerameters();
        finishLine = GameObject.Find("FinishLine");
        finishLine.transform.position = new Vector2(0, _levelLength);

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
        // Generates obstacles, healthUps, and currency based on lane length

        for (int i = 0; i < lane.Length; i++)
        {
            generationDistance = 0;

                for (int j = 0; j < _levelLength; j++)
                {
                    while (generationDistance < _levelLength)
                    {
                        // uses rng to randomise each spawned object

                        int nextObject = UnityEngine.Random.Range(rngMin, rngMax);
                        generationDistance += UnityEngine.Random.Range(minObstacleSpacing, maxObstacleSpacing);

                        
                        if (nextObject >= rngMax - 1)
                        {
                            // spawn a new healthUp

                            GameObject temp = Instantiate(HealthUpPrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                        else if (nextObject > rngMax - 5)
                        {
                            // spawn a new currency item

                            GameObject temp = Instantiate(CurrencyPrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                        else
                        {
                            if (LevelManager.levelManager.activeLevel != 1 && nextObject > rngMax - 8)
                            {
                                // spawn a new tall obstacle
                                GameObject temp = Instantiate(TallObstaclePrefab, lane[i].transform);
                                temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                                obstacleLists[i, j] = temp;
                            }
                            else
                            {
                                // spawn a new obstacle
                                GameObject temp = Instantiate(ObstaclePrefab, lane[i].transform);
                                temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                                obstacleLists[i, j] = temp;
                            }
                        }
                    }

                }
            

        }
    }

    public void UpdateGenerationPerameters()
    {
        switch (LevelManager.levelManager.activeLevel)
        {
            case 1:
                _levelLength = LevelManager.levelManager.activeLevel * defaultLength;
                minObstacleSpacing = defaultMinObstacleSpacing;
                maxObstacleSpacing = defaultMaxObstacleSpacing;
                rngMax = defaultRngMax;
                finishLine.transform.position = new Vector2(0, _levelLength);
                break;

            case 2:
                _levelLength = LevelManager.levelManager.activeLevel * defaultLength;
                minObstacleSpacing = defaultMinObstacleSpacing - 2;
                maxObstacleSpacing = defaultMaxObstacleSpacing - 4;
                rngMax = defaultRngMax + 1;
                finishLine.transform.position = new Vector2(0, _levelLength);
                break;

            case 3:
                _levelLength = LevelManager.levelManager.activeLevel * defaultLength;
                minObstacleSpacing = defaultMinObstacleSpacing - 3;
                maxObstacleSpacing = defaultMaxObstacleSpacing - 6;
                rngMax = defaultRngMax + 2;
                finishLine.transform.position = new Vector2(0, _levelLength);
                break;
        }

    }

    public void Reset()
    {

        // Destroys all existing lane objects and then repopulates the lanes

        for (int i = 0; i < lane.Length; i++)
        {
            for (int j = 0; j < lane[i].transform.childCount; j++)
            {
                //Debug.Log(i + " " + j);
                Destroy(lane[i].transform.GetChild(j).gameObject);
            }
        }
        Debug.Log("Cleared Array");
        UpdateGenerationPerameters();
        PopulateLanes();
    }

    
}
