using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : Obstacle
{
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
            // change light radius
            return false;
        }
        else
        {
            if (inputVal > minInput)
            {
                gameObject.SetActive(false);
                Photoresistor.Instance.LightOn();
                DrawGrid.Instance.DeleteFromMaze(gameObject.transform.position, false);
                return true;
            }
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            Photoresistor.Instance.LightShrink();
            SoundMgr.Instance.PlayAudio("CHARACTER_DIZZY_SFX_v1");
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
}
