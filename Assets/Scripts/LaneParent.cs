
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneParent : MonoBehaviour
{
    private int numberOfLanes = 5;
    public GameObject[] lane;
    private GameObject[,]obstacleLists;
    public GameObject ObstaclePrefab;
    public GameObject TallObstaclePrefab;
    public GameObject CurrencyPrefab;
    public GameObject GemPrefab;
    public GameObject HealthUpPrefab;
    public GameObject finishLine;
    private float minObstacleSpacing = 8.0f;
    private float maxObstacleSpacing = 20.0f;
    private int rngMax = 20;
    private int rngMin = 0;
    private float _levelLength;
    private float defaultLength = 750;
    private float defaultMinObstacleSpacing = 8.0f;
    private float defaultMaxObstacleSpacing = 20.0f;
    private int defaultRngMax = 20;
    public float levelLength
    {
        get { return _levelLength; }
    }
    private float generationDistance;
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
                    while (generationDistance < _levelLength - 20)
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
                        else if (nextObject >= rngMax - 2)
                        {
                            // spawn a new gem item
                            GameObject temp = Instantiate(GemPrefab, lane[i].transform);
                            temp.transform.position = new Vector2(lane[i].transform.position.x, generationDistance);
                            obstacleLists[i, j] = temp;
                        }
                        else if (nextObject > rngMax - 6)
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

    private void UpdateGenerationPerameters()
    {
        //Updates level generation perameters based on the active level
        switch (LevelManager.levelManager.activeLevel)
        {
            case 1:
                _levelLength = defaultLength;
                minObstacleSpacing = defaultMinObstacleSpacing;
                maxObstacleSpacing = defaultMaxObstacleSpacing;
                rngMax = defaultRngMax;
                finishLine.transform.position = new Vector2(0, _levelLength);
                break;

            case 2:
                _levelLength = defaultLength + (defaultLength / 3);
                minObstacleSpacing = defaultMinObstacleSpacing - (LevelManager.levelManager.activeLevel - 1);
                maxObstacleSpacing = defaultMaxObstacleSpacing - LevelManager.levelManager.activeLevel;
                rngMax = defaultRngMax + (LevelManager.levelManager.activeLevel - 1);
                finishLine.transform.position = new Vector2(0, _levelLength);
                break;

            case 3:
                _levelLength = defaultLength + (2 *(defaultLength / 3));
                minObstacleSpacing = defaultMinObstacleSpacing - LevelManager.levelManager.activeLevel;
                maxObstacleSpacing = defaultMaxObstacleSpacing - (LevelManager.levelManager.activeLevel + 1);
                rngMax = defaultRngMax + (LevelManager.levelManager.activeLevel - 1);
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
