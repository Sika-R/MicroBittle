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
    public static PlayerMovement Instance;
    bool _isFreezing = false;
    public bool isFreezing { get { return _isFreezing; } }
    bool isMoving = false;
    Vector2 curIdx;
    Vector3 origPos;
    Vector3 targetPos;
    [SerializeField]
    float timeToMove = 0.1f;
    public ObstacleType canPass = ObstacleType.None;
    [SerializeField] float freezeTime = 1.0f;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
#if UNITY_EDITOR
#else
        // WebGLDeviceConnection.Instance.SendStartLine();
#endif
        curIdx = DrawGrid.Instance.GetIdx(transform.position);
        Vector3 pos = DrawGrid.Instance.IdentifyCenter(transform.position);
        pos.y += GetComponent<CapsuleCollider>().height / 2;
        transform.position = pos;
        // Debug.Log(curIdx);
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
        if (_isFreezing)
        {
            return;
        }
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
            if(DrawGrid.Instance.canMove(destination, canPass))
            {
                StartCoroutine(MovePlayer(Vector3.forward));
                curIdx = destination;
            }
            Quaternion q = Quaternion.Euler(0, 0, 0);
            transform.rotation = q;
        }
        else if(direction == MovementDirections.Down && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x, curIdx.y - 1);
            if(DrawGrid.Instance.canMove(destination, canPass))
            {
                StartCoroutine(MovePlayer(-Vector3.forward));
                curIdx = destination;
            }
            Quaternion q = Quaternion.Euler(0, 180, 0);
            transform.rotation = q;
        }
        else if(direction == MovementDirections.Left && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x - 1, curIdx.y);
            if(DrawGrid.Instance.canMove(destination, canPass))
            {
                StartCoroutine(MovePlayer(-Vector3.right));
                curIdx = destination;
            }
            Quaternion q = Quaternion.Euler(0, 270, 0);
            transform.rotation = q;
        }
        else if(direction == MovementDirections.Right && !isMoving)
        {
            Vector2 destination = new Vector2(curIdx.x + 1, curIdx.y);
            if(DrawGrid.Instance.canMove(destination, canPass))
            {
                StartCoroutine(MovePlayer(Vector3.right));
                curIdx = destination;
            }
            Quaternion q = Quaternion.Euler(0, 90, 0);
            transform.rotation = q;
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
        SoundMgr.Instance.PlayAudio("CHARACTER_MOVING_SFX_v1");
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

    public void PlayerFreeze()
    {
        if (_isFreezing)
        {
            return;
        }
        StartCoroutine(FreezePlayer());
    }

    private IEnumerator FreezePlayer()
    {
        _isFreezing = true;
        yield return new WaitForSeconds(freezeTime);
        _isFreezing = false;
    }
}
