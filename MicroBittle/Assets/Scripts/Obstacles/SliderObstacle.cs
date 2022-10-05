using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderObstacle : Obstacle
{
    [SerializeField] int startValue;
    [SerializeField] int endValue;
    [SerializeField] int slideTime;
    private bool isInCoroutine;
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
    private void OnCollisionEnter(Collision collision)
    {
        collisonEvent(collision);
    }
    public override bool getInput(float inputVal)
    {
        if (!isInCoroutine)
        {
            //Debug.Log("is not in Coroutine, value is: " + Mathf.Floor(inputVal).ToString());
            if (Mathf.Floor(inputVal) <= startValue)
            {
                StartCoroutine(slideCoroutine());
            }
        }
        else
        {
            //Debug.Log("isInCoroutine, value is: " + Mathf.Floor(inputVal).ToString());
            if (Mathf.Floor(inputVal) >= endValue)
            {
                gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    IEnumerator slideCoroutine()
    {
        isInCoroutine = true;
        yield return new WaitForSeconds(slideTime);
        isInCoroutine = false;
    }

}
