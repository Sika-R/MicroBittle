using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMgr : MonoBehaviour
{
    public static ObstacleMgr Instance = null; 
    public List<GameObject> obstacles;
    public List<GameObject> obstaclePrefabs;
    private int currentObstacleIndex = 0;
    private Obstacle currentEncounteredObstacle = null;
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
        if(!CreativeMgr.Instance.canAddObstacle())
        {
            return;
        }
        //Debug.Log();
        //Debug.Log("Prefabs/Obstacles/Test" + obstacleType.ToString() + "Obstacle");
        //Debug.Log("Test" + obstacleType.ToString() + "Obstacle");
        //GameObject newObstacle_ = Resources.Load("enemy") as GameObject;
        GameObject newObstacle = Instantiate(obstaclePrefabs[(int)obstacleType]);
        //GameObject newObstacle = Instantiate(Resources.Load<P>("Prefabs/Obstacles/Test" + obstacleType.ToString() + "Obstacle"));
        Obstacle obstacle = newObstacle.GetComponent<Obstacle>();
        obstacle.isMovingWithMouse = true;
        obstacle.index = currentObstacleIndex;
        obstacle.obstacleType = obstacleType;
        obstacle.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        currentObstacleIndex++;
    }

    public void setCurrentEncounteredObstacle(Obstacle currentEncounteredObstacle_)
    {
        currentEncounteredObstacle = currentEncounteredObstacle_;
    }

    public void getInput(float inputVal, ObstacleType obstacleType)
    {
        if (PlayerMovement.Instance && PlayerMovement.Instance.isFreezing)
        {
            return;
        }
        // TODO:Fix this!!!
        //if(OutfitMgr.Instance.currentObstacleType == ObstacleType.Humid && obstacleType == ObstacleType.Humid)
        //{
        //    for(int i = 0; i < Mathf.Min(3, inputVal / 10); i++)
        //    {
        //        OutfitMgr.Instance.PutOnDivingSuit(i);
        //    }
        //    for(int i = ((int)inputVal) / 10; i < 3; i++)
        //    {
        //        OutfitMgr.Instance.TakeOffDivingSuitByPiece(i);
        //    }
        //
        //}
        
        // OutfitMgr.Instance.chooseToolIcon(obstacleType);
        if (obstacleType == ObstacleType.Light)
        {
            if(Photoresistor.Instance)
            {
                Photoresistor.Instance.currentLightVal = inputVal;
            }
            
            if (inputVal < 10)
            {
                if (Photoresistor.Instance)
                {
                    // SoundMgr.Instance.PlayAudio("HEADLAMP_ON_OFF_v1");
                    Photoresistor.Instance.LightOff();
                }
                
            }
            else
            {
                if (currentEncounteredObstacle && currentEncounteredObstacle.obstacleType != ObstacleType.Vacuum)
                {
                    if (Photoresistor.Instance)
                    {
                        // SoundMgr.Instance.PlayAudio("HEADLAMP_ON_OFF_v1");
                        Photoresistor.Instance.LightOn();
                    }
                }
                else if(currentEncounteredObstacle == null)
                {
                    if (Photoresistor.Instance)
                    {
                        // SoundMgr.Instance.PlayAudio("HEADLAMP_ON_OFF_v1");
                        Photoresistor.Instance.LightOn();
                    }
                }
            }
        }
        if (currentEncounteredObstacle == null)
        {
            return;
        }
        //if (obstacleType != currentEncounteredObstacle.obstacleType)
        //{
        //    return;
        //}
        if (currentEncounteredObstacle.getInput(inputVal, obstacleType))
        {
            currentEncounteredObstacle = null;
        }
    }
}
