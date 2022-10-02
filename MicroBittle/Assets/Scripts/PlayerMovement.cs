using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool isMoving = false;
    Vector3 origPos;
    Vector3 targetPos;
    [SerializeField]
    float timeToMove = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = DrawGrid.Instance.IdentifyCenter(transform.position);
        pos.y += GetComponent<CapsuleCollider>().height / 2;
        transform.position = pos;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.forward));
        }
        else if(Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(-Vector3.forward));
        }
        else if(Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(-Vector3.right));
        }
        else if(Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
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
