using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMgr : MonoBehaviour
{
    public static ObstacleMgr Instance = null; 
    public List<GameObject> obstacles;
    public List<GameObject> obstaclePrefabs;
    public GameObject obstaclePrefab;
    private int currentObstacleIndex = 0;
    private Obstacle currentEncounteredObstacle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreateNewObstacles(ObstacleType obstacleType)
    {
        //Debug.Log();
        //Debug.Log("Prefabs/Obstacles/Test" + obstacleType.ToString() + "Obstacle");
        //Debug.Log("Test" + obstacleType.ToString() + "Obstacle");
        //GameObject newObstacle_ = Resources.Load("enemy") as GameObject;
        GameObject newObstacle = Instantiate(obstaclePrefabs[(int)obstacleType]);
        //GameObject newObstacle = Instantiate(Resources.Load<P>("Prefabs/Obstacles/Test" + obstacleType.ToString() + "Obstacle"));
        Obstacle obstacle = newObstacle.GetComponent<Obstacle>();
        obstacle.index = currentObstacleIndex;
        obstacle.obstacleType = obstacleType;
        currentObstacleIndex++;
    }

    public void setCurrentEncounteredObstacle(Obstacle currentEncounteredObstacle_)
    {
        currentEncounteredObstacle = currentEncounteredObstacle_;
    }

    public void getInput(float inputVal, ObstacleType obstacleType)
    {
        if (currentEncounteredObstacle == null)
        {
            return;
        }
        if (obstacleType != currentEncounteredObstacle.obstacleType)
        {
            return;
        }
        if (currentEncounteredObstacle.getInput(inputVal))
        {
            currentEncounteredObstacle = null;
        }
    }
}
