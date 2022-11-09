using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObstacle : Obstacle
{
    // Start is called before the first frame update
    [SerializeField] float minVal = 10f;
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
        // PlayerMovement.Instance.PlayerFreeze();
        SoundMgr.Instance.PlayAudio("mouseScream");
        SoundMgr.Instance.PlayAudio("CHARACTER_SCARED_SFX_v1");
    }

    public override bool getInput(float inputVal, ObstacleType obstacleType)
    {
        /*
        if(obstacleType == OutfitMgr.Instance.currentObstacleType)
        {
            if (obstacleType == ObstacleType.Light)
            {
                if (inputVal > minInput)
                {
                    ScarePlayer();
                    PlayerMovement.Instance.canPass = ObstacleType.None;
                    OutfitMgr.Instance.headLight.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    PlayerMovement.Instance.canPass = ObstacleType.Light;
                    OutfitMgr.Instance.headLight.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        */

        /*else
        {
            if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Light)
            // if (obstacleType == ObstacleType.Slider)
            {
                PlayerMovement.Instance.canPass = ObstacleType.None;
                if (inputVal > minInput)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) <= playerCollideRadius)
                    {
                        PlayerMovement.Instance.canPass = ObstacleType.None;
                        ScarePlayer();
                    }
                    //scream
                }
                else
                {
                    PlayerMovement.Instance.canPass = ObstacleType.Light;
                }
                return false;
            }
        }*/
        
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            if (Photoresistor.Instance.currentLightVal > minInput)
            {
                PlayerMovement.Instance.canPass = ObstacleType.None;
                //OutfitMgr.Instance.headLight.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                PlayerMovement.Instance.canPass = ObstacleType.Light;
                //OutfitMgr.Instance.headLight.transform.GetChild(0).gameObject.SetActive(false);
            } 
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        if (collider.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            if (Photoresistor.Instance.currentLightVal > minInput)
            {
                ScarePlayer();
            }
        }
    }

    public override void SetBoundary(List<float> values)
    { 
        minInput = (int)values[0];
    }
}
