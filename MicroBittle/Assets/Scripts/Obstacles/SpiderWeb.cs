using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : Obstacle
{
    // Start is called before the first frame update
    bool isAudioPlayed = false;
    void Start()
    {
        InitializeObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdate();
    }

    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (obstacleType != this.obstacleType || OutfitMgr.Instance.currentObstacleType != this.obstacleType)
        {
            // change light radius
            return false;
        }
        else
        {
            if (inputVal > minInput)
            {
                
                if(SoundMgr.Instance)
                {
                    SoundMgr.Instance.PlayAudio("VACUUM_CLEANER_WORKING_SHORT");
                }
                
                Invoke("disappear", 0.5f);
                return true;
            }
        }
        return false;
    }

    private void disappear()
    {
        gameObject.SetActive(false);
        if(DrawGrid.Instance)
        {
            DrawGrid.Instance.DeleteFromMaze(gameObject.transform.position, false);
        }
        if (ProgramUIMgr.Instance)
        {
            ProgramUIMgr.Instance.AddSuccess();
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            //if (isAudioPlayed)
            //{
            //    return;
            //}
            //isAudioPlayed = true;
            //StartCoroutine(SetAudioPlayedFalse());
            //SoundMgr.Instance.PlayAudio("CHARACTER_DIZZY_SFX_v1");
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        if (collider.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            if (SoundMgr.Instance)
            {
                SoundMgr.Instance.PlayOnce("CHARACTER_DIZZY_SFX_v1");
            }
            
            Debug.Log("play audio!");
        }   
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            if (Photoresistor.Instance.currentLightVal > 10)
            {
                Photoresistor.Instance.LightOn();
            }
            exitTriggerEvent(other);
        }
    }

    private IEnumerator SetAudioPlayedFalse()
    {
        yield return new WaitForSeconds(3);
        isAudioPlayed = false;
    }

    public override void SetBoundary(List<float> values)
    {
        // startValue = (int)values[0];
        // endValue = (int)values[1];
        minInput = (int)values[0];
        // slideTime = (int)values[2];
    }

    public override void TryInit()
    {
        if (ParamManager.Instance)
        {
            if (ParamManager.Instance.GetParamByFunction(FunctionType.vacuum) != null)
            {
                SetBoundary(ParamManager.Instance.GetParamByFunction(FunctionType.vacuum));
            }

        }
    }
}
