using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObstacle : Obstacle
{
    // Start is called before the first frame update
    [SerializeField] float radius;
    SphereCollider myCollider;
    [SerializeField] float playerCollideRadius;
    GameObject player;

    void Start()
    {
        InitializeObstacle();
        myCollider = GetComponent<SphereCollider>();
        myCollider.radius = radius;
        player = GameObject.FindGameObjectWithTag("Player");
        if(ParamManager.Instance)
        {
            SetBoundary(ParamManager.Instance.GetParamByFunction(FunctionType.headlamp));
        }
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdate();
    }

    private void ScarePlayer()
    {
        PlayerMovement.Instance.PlayerFreeze();
        SoundMgr.Instance.PlayAudio("mouseScream");
        SoundMgr.Instance.PlayAudio("teethChatter");
    }

    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (obstacleType == ObstacleType.Slider)
        {
            if (inputVal > minInput)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= playerCollideRadius)
                {
                    ScarePlayer();
                }
                //scream

            }
            return false;
        }
        else if (obstacleType == ObstacleType.Light)
        {
            if (inputVal > minInput)
            {
                 ScarePlayer();
            }
        }
        return false;
    }

    public override void SetBoundary(List<float> values)
    { 
        minInput = (int)values[0];
    }
}
