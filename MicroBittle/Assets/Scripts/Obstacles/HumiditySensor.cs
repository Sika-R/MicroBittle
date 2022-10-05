using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumiditySensor : Obstacle
{
    [SerializeField] List<int> inputValues;
    // Start is called before the first frame update
    void Start()
    {
        InitializeObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdate();
    }

    public override bool getInput(float inputVal)
    {
        for (int i = 0; i < inputValues.Count; ++i)
        {
            if (inputVal < inputValues[i])
            {
                return false;
            }
            OutfitMgr.Instance.PutOnDivingSuit(i);
        }
        gameObject.SetActive(false);
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisonEvent(collision);
    }

}
