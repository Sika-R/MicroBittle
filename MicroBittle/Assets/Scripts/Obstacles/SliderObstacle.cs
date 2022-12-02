using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliderObstacle : Obstacle
{
    [SerializeField] float slideTime;
    private bool isInCoroutine;
    private float slidingTime;
    private float startSlideTime;
    private float startValue;
    private float endValue;
    // Start is called before the first frame update
    void Start()
    {
        InitializeObstacle();
        TryInit();
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdate();
        if (isInCoroutine)
        {
            slidingTime = Time.time - startSlideTime;
            //Debug.Log("slidingTime: " + slidingTime);
            //Debug.Log("slidingTime: " + slidingTime + " start value: " + startValue + " end value: " + endValue + " changed value: " + Mathf.Abs(startValue - endValue));
            if (Mathf.Abs(startValue - endValue) >= minInput)
            {
                destroyRock();
                isInCoroutine = false;
                //Debug.Log("destroy rock!!!!!");
            }
            if (slidingTime >= slideTime)
            {
                isInCoroutine = false;
                //Debug.Log("============time out==========");
            }
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        collisonEvent(collision);
    }*/
    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (obstacleType != this.obstacleType)
        {
            return false;
        }
        if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Slider) return false;

        if (!isInCoroutine)
        {
            isInCoroutine = true;
            startValue = inputVal;
            endValue = inputVal;
            //Debug.Log("start val: " + startValue);
            resetTimer();
        }
        else
        {
            endValue = inputVal;
            //Debug.Log("end value: " + endValue);
        }
        return false;
    }

    private void resetTimer()
    {
        startSlideTime = Time.time;
        slidingTime = 0;
    }

    private void destroyRock()
    {
        if(CameraShake.Instance)
        {
            CameraShake.Shake(0.1f, 0.05f);
        }
        
        if (transform.Find("explosion"))
        {
            transform.Find("explosion").gameObject.SetActive(true);
        }
        if (transform.Find("rock1"))
        {
            transform.Find("rock1").gameObject.SetActive(false);
        }
        MeshDestroy meshDestroy = GetComponentInChildren(typeof(MeshDestroy)) as MeshDestroy;
        if (meshDestroy != null)
        {
            try
            {
                meshDestroy.DestroyMesh();
            } catch (Exception e)
            {
                //nothing
            }
            
        }
        if (SoundMgr.Instance)
        {
            SoundMgr.Instance.PlayAudio("CHARACTER_BREAK_SFX_v1");
        }
        if(DrawGrid.Instance)
        {
            DrawGrid.Instance.DeleteFromMaze(gameObject.transform.position, false);
        }
        if (ProgramUIMgr.Instance)
        {
            ProgramUIMgr.Instance.AddSuccess();
            gameObject.SetActive(false);
        }
        if(DialogueControllerProgramFlow_StoryMode.Instance)
        {
            programUI.Instance.setDemoWork();
        }
        Invoke("SetUnactive", 1.0f);

        /*if (programUI.Instance)
        {
            programUI.Instance.setDemoWork();
        }*/

    }

    void SetUnactive()
    {
        gameObject.SetActive(false);
    }

    IEnumerator slideCoroutine()
    {
        isInCoroutine = true;
        yield return new WaitForSeconds(slideTime);
        isInCoroutine = false;
    }

    public override void SetBoundary(List<float> values)
    {
        // startValue = (int)values[0];
        // endValue = (int)values[1];
        minInput = (int)values[1] - values[0];
        // slideTime = (int)values[2];
    }

    public override void TryInit()
    {
        if (ParamManager.Instance)
        {
            if (ParamManager.Instance.GetParamByFunction(FunctionType.powerlog) != null)
            {
                SetBoundary(ParamManager.Instance.GetParamByFunction(FunctionType.powerlog));
            }

        }
    }

}
