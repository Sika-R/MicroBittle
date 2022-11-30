using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobObstacle : Obstacle
{
    [SerializeField] List<float> knobValueRange;
    [SerializeField] List<GameObject> rocks;
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

    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (obstacleType != this.obstacleType)
        {
            return false;
        }
        //if (OutfitMgr.Instance.currentObstacleType != this.obstacleType) return false;
        for (int i = 1; i < knobValueRange.Count; ++i)
        {
            if (inputVal >= knobValueRange[i - 1] && inputVal < knobValueRange[i]) {
                DestroyRock(i-1);
                if (isAllRockDestroyed())
                {
                    if(DrawGrid.Instance)
                    {
                        DrawGrid.Instance.DeleteFromMaze(gameObject.transform.position, false);
                    }
                    if (ProgramUIMgr.Instance)
                    {
                        ProgramUIMgr.Instance.AddSuccess();
                    }

                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyRock(int index)
    {
        if (rocks[index] == null) 
        {
            return;
        }
        if(CameraShake.Instance)
        {
            CameraShake.Shake(0.1f, 0.05f);
        }
        
        if (transform.Find("explosion"))
        {
            transform.Find("explosion").gameObject.GetComponent<ParticleSystem>().Play();
        }
        if (SoundMgr.Instance)
        {
            SoundMgr.Instance.PlayAudio("CHARACTER_BREAK_SFX_v1");
        }
        
        rocks[index].SetActive(false);
        rocks[index] = null;
    }

    private bool isAllRockDestroyed()
    {
        foreach (var rock in rocks)
        {
            if (rock != null)
            {
                return false;
            }
        }
        return true;
    }

    public override void SetBoundary(List<float> values)
    {
        knobValueRange[1] = values[0];
        knobValueRange[2] = values[1];
    }


    public override void TryInit()
    {
        if (ParamManager.Instance)
        {
            if (ParamManager.Instance.GetParamByFunction(FunctionType.jackhammer) != null)
            {
                Debug.Log(">>>>>");
                SetBoundary(ParamManager.Instance.GetParamByFunction(FunctionType.jackhammer));
            }

        }
    }
}
