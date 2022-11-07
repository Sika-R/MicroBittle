using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DrawGrid : MonoBehaviour
{
    public static DrawGrid Instance;
    [SerializeField]
    MazeInformation originalMaze;
    [SerializeField]
    MazeInformation maze;
	[SerializeField]
    bool bShowGizmos = true;

    [SerializeField]
    int m_column = 9; //列
    [SerializeField]
    int m_row = 9; //行
    int prevColumn;
    int preRow;
     
 	[SerializeField]
    public float m_gridSize = 1.0f; //大概的大小
    private readonly Color m_color = Color.white;

    private BoxCollider m_collider; //用来做射线检测

    [HideInInspector]
    public GameObject m_obstaclePrefab;

    private void Awake()
    {
        Instance = this;
    }
 	
 	private void Start()
    {
        maze = Instantiate<MazeInformation>(originalMaze);
        //gameObject.layer = 31;
        m_collider = GetComponent<BoxCollider>();
        if (m_collider == null)
        {
            m_collider = gameObject.AddComponent<BoxCollider>();
            m_collider.size = new Vector3(100, 0.1f, 100);
        }
    }

    private void OnValidate()
    {
        maze.SetCol(m_column);
        maze.SetRow(m_row);
    }

    private void GridGizmo(int cols, int rows)
    {
        Vector3 start;
        Vector3 end;
        for (int i = 0; i <= cols; i++)
        {
            start = new Vector3(i * m_gridSize - cols / 2 * m_gridSize, 0, -rows / 2 * m_gridSize);
            end = new Vector3(i * m_gridSize - cols / 2 * m_gridSize, 0, rows / 2 * m_gridSize);
            Gizmos.DrawLine(transform.position + start, transform.position + end);
        }
        for (int j = 0; j <= rows; j++)
        {
            start = new Vector3(- cols / 2 * m_gridSize, 0, -rows / 2 * m_gridSize + j * m_gridSize);
            end = new Vector3(cols * m_gridSize - cols / 2 * m_gridSize, 0, -rows / 2 * m_gridSize + j * m_gridSize);
            Gizmos.DrawLine(transform.position + start, transform.position + end);
        }
    }
 
    private void OnDrawGizmos()
    {
    	if (bShowGizmos == false || Application.isEditor == false )
    	{
            return;
    	}
        Gizmos.color = m_color;
        GridGizmo(m_column, m_row);
    }

    public Vector3 IdentifyCenter(Vector3 hit)
    {
        float x = hit.x - transform.position.x;
        float z = hit.z - transform.position.z;
    	/*if(x < -m_column * m_gridSize / 2 || x > m_column * m_gridSize / 2 || z < -m_row * m_gridSize / 2 || z > m_row * m_gridSize / 2)
    	{
    		return new Vector3(-1, -1, -1);
    	}*/
    	int xx = (int)Mathf.Floor(x / m_gridSize);
    	int zz = (int)Mathf.Floor(z / m_gridSize);
        Debug.Log("x: " + xx + "z: " + zz);
        Vector3 position = new Vector3((xx + 0.5f) * m_gridSize, 0, (zz + 0.5f) * m_gridSize);
    	return position + transform.position;
    }
    private Vector3 GetCenterPos(int x, int z)
    {
        Vector3 position = new Vector3((x + 0.5f) * m_gridSize, 0, (z + 0.5f) * m_gridSize);
        return position + transform.position;
    }

    public Vector2 GetIdx(Vector3 hit)
    {
        float x = hit.x - transform.position.x;
        float z = hit.z - transform.position.z;
        /*if(x < -m_column * m_gridSize / 2 || x > m_column * m_gridSize / 2 || z < -m_row * m_gridSize / 2 || z > m_row * m_gridSize / 2)
        {
            return new Vector2(-99, -99);
        }*/
        int xx = (int)Mathf.Floor(x / m_gridSize);
        int zz = (int)Mathf.Floor(z / m_gridSize);
        // Debug.Log("x: " + xx + "z: " + zz);
        Vector2 vec = new Vector2(xx, zz);
        return vec;
    }

    public Vector3 EditMaze(Vector3 hit, ObstacleType type)
    {
        // EditorUtility.SetDirty(maze);
        Vector2 idx = GetIdx(hit);
        if(idx.x == -99)
        {
            return new Vector3(-99, -99, -99);
        }
        Vector3 gridPos = GetCenterPos((int)idx.x, (int)idx.y);
        if(originalMaze.ContainsKey(idx))
        {
            originalMaze[idx].SetObstacle(type);
            // Debug.Log("grid exists.");
            
        }
        else
        {
            Grid grid = new Grid((int)idx.x, (int)idx.y);
            grid.SetObstacle(type);
            originalMaze.Add(idx, grid);
            // Debug.Log(maze.Count);
            Debug.Log("X: " + grid.x + "Y: " + grid.y);
            // Debug.Log("X: " + maze[idx].x + "Y: " + maze[idx].y);
        }
        return gridPos;
    }

    public Vector3 DeleteFromMaze(Vector3 hit, bool isFloor)
    {
        // EditorUtility.SetDirty(maze);
        Vector2 idx = GetIdx(hit);
        if(idx.x == -99)
        {
            return new Vector3(-99, -99, -99);
        }
        Vector3 gridPos = GetCenterPos((int)idx.x, (int)idx.y);

        if(maze.ContainsKey(idx))
        {
            if(isFloor)
            {
                maze.Remove(idx);
            }
            else
            {
                maze[idx].SetObstacle(ObstacleType.None);
            }
            // Debug.Log("grid exists.");
            
        }
        return gridPos;
    }

    public Vector3 EditorDeleteFromMaze(Vector3 hit, bool isFloor)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(maze);
        AssetDatabase.SaveAssets();
#endif
        Vector2 idx = GetIdx(hit);
        if(idx.x == -99)
        {
            return new Vector3(-99, -99, -99);
        }
        Vector3 gridPos = GetCenterPos((int)idx.x, (int)idx.y);

        if(originalMaze.ContainsKey(idx))
        {
            if(isFloor)
            {
                originalMaze.Remove(idx);
            }
            else
            {
                originalMaze[idx].SetObstacle(ObstacleType.None);
            }
            // Debug.Log("grid exists.");
            
        }
        return gridPos;
    }

    public bool canMove(Vector2 idx, ObstacleType curType)
    {
        if(maze.ContainsKey(idx))
        {
            if(maze[idx].obstacle == ObstacleType.None)
            {
                return true;
            }
            if(curType == maze[idx].obstacle)
            {
                return true;
            }
        }
        return false;
    }
}

