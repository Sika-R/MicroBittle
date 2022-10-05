using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirections
{
    Null = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}

public class PlayerMovement : MonoBehaviour
{

    bool isMoving = false;
    Vector2 curIdx;
    Vector3 origPos;
    Vector3 targetPos;
    [SerializeField]
    float timeToMove = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        curIdx = DrawGrid.Instance.GetIdx(transform.position);
        Vector3 pos = DrawGrid.Instance.IdentifyCenter(transform.position);
        pos.y += GetComponent<CapsuleCollider>().height / 2;
        transform.position = pos;
        if(WebGLDeviceConnection.Instance.movementEvent != null)
        {
            Debug.Log("Add listener");
            WebGLDeviceConnection.Instance.movementEvent.AddListener(StartMovement);
        }
        // 
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartMovement(MovementDirections.Up);
            // StartCoroutine(MovePlayer(Vector3.forward));
        }
        else if(Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartMovement(MovementDirections.Down);
            // StartCoroutine(MovePlayer(-Vector3.forward));
        }
        else if(Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartMovement(MovementDirections.Left);
            // StartCoroutine(MovePlayer(-Vector3.right));
        }
        else if(Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartMovement(MovementDirections.Right);
            // StartCoroutine(MovePlayer(Vector3.right));
        }
    }

    private void StartMovement(MovementDirections direction)
    {
        if(direction == MovementDirections.Up && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x, curIdx.y + 1);
            if(DrawGrid.Instance.canMove(destination))
            {
                StartCoroutine(MovePlayer(Vector3.forward));
                curIdx = destination;
            }
        }
        else if(direction == MovementDirections.Down && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x, curIdx.y - 1);
            if(DrawGrid.Instance.canMove(destination))
            {
                StartCoroutine(MovePlayer(-Vector3.forward));
                curIdx = destination;
            }
        }
        else if(direction == MovementDirections.Left && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x - 1, curIdx.y);
            if(DrawGrid.Instance.canMove(destination))
            {
                StartCoroutine(MovePlayer(-Vector3.right));
                curIdx = destination;
            }
        }
        else if(direction == MovementDirections.Right && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x + 1, curIdx.y);
            if(DrawGrid.Instance.canMove(destination))
            {
                StartCoroutine(MovePlayer(Vector3.right));
                curIdx = destination;
            }
        }

    }

    /*private bool validMove(Vector2 destination)
    {
        return canMove(destination);
    }*/

    private IEnumerator MovePlayer(Vector3 direction)
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction * DrawGrid.Instance.m_gridSize;
        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}
