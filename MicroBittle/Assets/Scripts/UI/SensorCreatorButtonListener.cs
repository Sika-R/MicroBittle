using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorCreatorButtonListener : MonoBehaviour
{
    public ObstacleType obstacleType;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => ObstacleMgr.Instance.CreateNewObstacles(obstacleType));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
