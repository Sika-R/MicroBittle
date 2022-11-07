using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType // *
{
    None,
    Humid,
    ButtonA,
    Light,
    Slider,
    Wall,
    Photoresistor,
    TrimmerPotentiometer,
    Vacuum
}

public enum InputType
{ 
    boolean,
    number,
}

public class Obstacle : MonoBehaviour
{
    public ObstacleType obstacleType;
    public InputType inputType;
    public float minInput;
    public int index;
    public bool isMovingWithMouse;
    public DrawGrid drawGridScript;
    // Start is called before the first frame update
    void Start()
    {
        InitializeObstacle();
    }

    // Update is called once per frame
    private void Update()
    {
        ObstacleUpdate();
    }

    public void InitializeObstacle()
    {
        // isMovingWithMouse = true;
        drawGridScript = GameObject.Find("Grid").GetComponent<DrawGrid>();
    }

    public void ObstacleUpdate()
    {
        if (!isMovingWithMouse)
        {
            return;
        }
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 2000);
            for (int i = 0; i < hits.Length; ++i)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.tag == "Floor")
                {
                    isMovingWithMouse = false;
                    //transform.position = new Vector3(transform.position.x, transform.position.y, hit.collider.transform.position.z);
                    Vector3 initPoint = drawGridScript.IdentifyCenter(hit.point);
                    // Vector3 initPoint = drawGridScript.EditMaze(hit.point, obstacleType);
                    transform.localScale = drawGridScript.m_gridSize * new Vector3(1, 1, 1);
                    //initPoint.y = GetComponent<BoxCollider>().size.y * transform.localScale.y / 2 * drawGridScript.m_gridSize;

                    transform.position = initPoint;

                    return;
                }
            }
        }
    }

    public virtual bool getInput(float inputVal, ObstacleType obstacleType)
    {
        if (inputVal > minInput)
        {
            // DrawGrid.Instance.DeleteFromMaze(transform.position, false);
            // gameObject.
            // gameObject.SetActive(false);
            return true;
        }
        return false;
    }


    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enter");
        collisonEvent(collision);
    }*/
    public void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Enter");
        collisonEvent(collider);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        exitTriggerEvent(other);
    }

    public void collisonEvent(Collider collider)
    // public void collisonEvent(Collision collision)
    {
        // if (collision.gameObject.tag == "Player" && !isMovingWithMouse)
        if(collider.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            ObstacleMgr.Instance.setCurrentEncounteredObstacle(this);
        }
    }

    public void exitTriggerEvent(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && !isMovingWithMouse)
        {
            ObstacleMgr.Instance.setCurrentEncounteredObstacle(null);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        return;
    }

    public virtual void SetBoundary(List<float> values)
    { 
        return;
    }
}
