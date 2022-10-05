using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorInputValueListener : MonoBehaviour
{
    [SerializeField] ObstacleType obstacleType;
    [SerializeField] float inputValue;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => ObstacleMgr.Instance.getInput(inputValue, obstacleType));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
