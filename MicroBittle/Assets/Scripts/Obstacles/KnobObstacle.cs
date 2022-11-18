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
        if (OutfitMgr.Instance.currentObstacleType != this.obstacleType) return false;
        for (int i = 1; i < knobValueRange.Count; ++i)
        {
            if (inputVal >= knobValueRange[i - 1] && inputVal < knobValueRange[i]) {
                DestroyRock(i-1);
                if (isAllRockDestroyed())
                {
                    DrawGrid.Instance.DeleteFromMaze(gameObject.transform.position, false);
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
        CameraShake.Shake(0.1f, 0.05f);
        if (transform.Find("explosion"))
        {
            transform.Find("explosion").gameObject.SetActive(true);
        }
        if (SoundMgr.Instance)
        {
            SoundMgr.Instance.PlayAudio("CHARACTER_BREAK_SFX_v1");
        }
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
}