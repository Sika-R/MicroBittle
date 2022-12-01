using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreativeMgr.Instance.maze.SetActive(true);
        CreativeMgr.Instance.maze.GetComponent<DrawGrid>().SetInstance();
        Transform startPlace = CreativeMgr.Instance.maze.transform.Find("Start");
        PlayerMovement.Instance.gameObject.transform.position = startPlace.position;
        PlayerMovement.Instance.Init();
        List<ObstacleType> allOutfit = OutfitMgr.Instance.allPossibleTypes;
        allOutfit.Clear();
        for(int i = 0; i < CreativeMgr.Instance.curObstacle.Count; i++)
        {
            ObstacleType obs = ObstacleToObstacleType(CreativeMgr.Instance.curObstacle[i]);
            if (obs != ObstacleType.None)
            {
                allOutfit.Add(obs);
            }
        }
        OutfitMgr.Instance.ChangeOutfit(true);
        OutfitMgr.Instance.ChangeOutfit(false);
        Destroy(CreativeMgr.Instance.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ObstacleType ObstacleToObstacleType(ParamManager.Obstacle o)
    {
        switch (o)
        {
            case ParamManager.Obstacle.wall:
                return ObstacleType.Slider;
                
            case ParamManager.Obstacle.rock :
                return ObstacleType.Knob;
                break;
            case ParamManager.Obstacle.spiderweb:
                return ObstacleType.Vacuum;
                break;
            case ParamManager.Obstacle.mouse:
                return ObstacleType.Light;
                break;
        }
        return ObstacleType.None;
    }
}
