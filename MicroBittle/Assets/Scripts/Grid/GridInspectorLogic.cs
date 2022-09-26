using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawGrid))]
public class GridInspectorLogic : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawGrid gridObj = (DrawGrid)target;
        gridObj.m_obstaclePrefab = EditorGUILayout.ObjectField("Choose Object to place",gridObj.m_obstaclePrefab,typeof(GameObject),true)as GameObject;

    }

    public void OnSceneGUI()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hitInfo;
        bool isDown = false;
        LayerMask mask = 1 << 9 | 1 << 10;
        if (Physics.Raycast(ray, out hitInfo, 2000, ~mask)) //这里设置层
        {
        	if(hitInfo.collider.tag == "Floor")
        	{
        		float x = hitInfo.point.x;
	            float y = hitInfo.point.y;
	            Event e = Event.current;
	            if (!isDown && e.type == EventType.KeyDown)
	            {
	                if (e.keyCode == KeyCode.LeftShift)
	                {
	                	isDown = true;
	                	GameObject prefab = ((DrawGrid)target).m_obstaclePrefab;
	                	Vector3 initPoint = ((DrawGrid)target).IdentifyCenter(hitInfo.point);
	                	initPoint.y += prefab.GetComponent<BoxCollider>().size.y * prefab.transform.localScale.y / 2 * ((DrawGrid)target).m_gridSize;

						GameObject newobj = Instantiate(prefab, initPoint, Quaternion.Euler(0, 0, 0), ((DrawGrid)target).transform) as GameObject;  //设置障碍 
						// Debug.Log(newobj.transform.localScale);
                		newobj.transform.localScale = ((DrawGrid)target).m_gridSize * newobj.transform.localScale;
                		Undo.RegisterCreatedObjectUndo(newobj, "NewObject");
	                }
	            }
        	}
        	else if(hitInfo.collider.tag == "Cube")
        	{
				float x = hitInfo.point.x;
	            float y = hitInfo.point.y;
	            Event e = Event.current;
	            if (!isDown && e.type == EventType.KeyDown)
	            {
	                if (e.keyCode == KeyCode.LeftControl)
	                {
	                	isDown = true;
	                	Undo.DestroyObjectImmediate(hitInfo.transform.gameObject);
	                }
	                else if(e.keyCode == KeyCode.LeftShift)
	                {
	                	isDown = true;
	                	Transform bottom = hitInfo.transform;
	                	DrawGrid drawgrid = bottom.gameObject.GetComponentInParent<DrawGrid>();
	                	GameObject prefab = drawgrid.m_obstaclePrefab;
	                	Vector3 initPoint = bottom.position;
	                	initPoint.y += bottom.gameObject.GetComponent<BoxCollider>().size.y * bottom.localScale.y / 2;
	                	initPoint.y += prefab.GetComponent<BoxCollider>().size.y * prefab.transform.localScale.y / 2 * ((DrawGrid)target).m_gridSize;
	                	GameObject newobj = Instantiate(prefab, initPoint, Quaternion.Euler(0, 0, 0), ((DrawGrid)target).transform) as GameObject;  //设置障碍 
                		newobj.transform.localScale = newobj.transform.localScale = ((DrawGrid)target).m_gridSize * newobj.transform.localScale;
                		Undo.RegisterCreatedObjectUndo(newobj, "NewObject");
	                }
	            }
        	}
            
        }
    }
}


