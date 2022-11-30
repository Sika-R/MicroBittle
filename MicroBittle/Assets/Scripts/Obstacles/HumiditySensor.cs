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
        if(ParamManager.Instance)
        {
            // SetBoundary(ParamManager.Instance.GetParamByFunction(FunctionType.divinggear));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdate();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Humid) return;
        if(!OutfitMgr.Instance.divingSuits[2].activeSelf)
        {
            PlayerMovement.Instance.canPass = ObstacleType.None;
            PlayerMovement.Instance.PlayerFreeze();
            SoundMgr.Instance.PlayAudio("teethChatter");
        }
        else
        {
            PlayerMovement.Instance.canPass = ObstacleType.Humid;
        }
        
    }
    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (obstacleType != this.obstacleType)
        {
            return false;
        }
        if(OutfitMgr.Instance.currentObstacleType != ObstacleType.Humid) return false;
        /*for (int i = 0; i < inputValues.Count; ++i)
        {
            if (inputVal < inputValues[i])
            {
                return false;
            }
            // OutfitMgr.Instance.PutOnDivingSuit(i);
        }*/
        if(!OutfitMgr.Instance.divingSuits[2].activeSelf) return false;
        PlayerMovement.Instance.canPass = ObstacleType.Humid;
        // gameObject.SetActive(false);
        return true;
    }

    public override void SetBoundary(List<float> values)
    { 
        inputValues[0] = (int)values[0];
        inputValues[1] = (int)values[3];
        inputValues[2] = (int)values[6];
    }


    /*private void OnCollisionEnter(Collision collision)
    {
        collisonEvent(collision);
    }*/

}
