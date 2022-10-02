using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class DrawGrid : MonoBehaviour
{
    public static DrawGrid Instance;

	[SerializeField]
    bool bShowGizmos = true;

    [SerializeField]
    int m_column = 9; //列
    [SerializeField]
    int m_row = 9; //行
 
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
        //gameObject.layer = 31;
        m_collider = GetComponent<BoxCollider>();
        if (m_collider == null)
        {
            m_collider = gameObject.AddComponent<BoxCollider>();
            m_collider.size = new Vector3(100, 0.1f, 100);
        }
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
    	if(x < -m_column * m_gridSize || x > m_column * m_gridSize / 2 || z < -m_row * m_gridSize / 2 || z > m_row * m_gridSize / 2)
    	{
    		return new Vector3(-1, -1, -1);
    	}
    	int xx = (int)Mathf.Floor(x / m_gridSize);
    	int zz = (int)Mathf.Floor(z / m_gridSize);
        Vector3 position = new Vector3((xx + 0.5f) * m_gridSize, 0, (zz + 0.5f) * m_gridSize);
    	return position + transform.position;
    }
}

