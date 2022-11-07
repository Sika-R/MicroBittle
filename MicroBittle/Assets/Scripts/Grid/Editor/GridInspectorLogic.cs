using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawGrid))]
public class GridInspectorLogic : Editor
{
	bool isDown = false;

	enum ActionType
	{
		Create,
		Destroy,
		Rotate,
	}
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawGrid gridObj = (DrawGrid)target;
       	gridObj.m_obstaclePrefab = EditorGUILayout.ObjectField("Choose Object to place",
						        	gridObj.m_obstaclePrefab,
						        	typeof(GameObject),
						        	false)as GameObject;
       	/*if (GUILayout.Button ("ShowObjectPicker")) {
            int controlID = EditorGUIUtility.GetControlID (FocusType.Passive);
            EditorGUIUtility.ShowObjectPicker<GameObject> (null, false, "", controlID);
        }
        gridObj.m_obstaclePrefab = EditorGUIUtility.GetObjectPickerObject() as GameObject;*/
    }

    void OnEnable() {
    	SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        Debug.Log("Start");
    }
  
    void OnDisable() {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        Debug.Log("End");
    }

    public void OnSceneGUI(SceneView sceneView)
    {
    	Event current = Event.current;
    	int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

    	switch (current.type)
	    {
	      	case EventType.MouseDown:
	      		current.Use();
		      	if(current.button == 0)
		      	{
		      		RaycastTo(ActionType.Create);
		      	}
		      	else if(current.button == 1)
		      	{
		      		RaycastTo(ActionType.Destroy);
		      	}
	        
	        
	        	break;
	        case EventType.KeyDown:
	        	current.Use();
	        	if(current.keyCode == KeyCode.LeftShift)
	        	{
	        		RaycastTo(ActionType.Rotate);
	        	}
	        	break;

	        case EventType.MouseDrag:
	        	// isDown = false;
	      		current.Use();
		      	if(current.button == 0)
		      	{
		      		RaycastTo(ActionType.Create);
		      	}
		      	/*else if(current.button == 1)
		      	{
		      		RaycastTo(ActionType.Destroy);
		      	}*/
	        
	        
	        	break;
	        case EventType.MouseUp:
	        	isDown = false;
	        	break;
	 
	      	case EventType.Layout:
	        	HandleUtility.AddDefaultControl(controlID);
	        	break;
	    }
    }

    void RaycastTo(ActionType action)
    {
    	Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hitInfo;
        //bool isDown = false;
        LayerMask mask = 1 << 9 | 1 << 10;
        
    	if(action == ActionType.Create)
    	{
    		if (Physics.Raycast(ray, out hitInfo, 2000, ~mask)) //这里设置层
	        {
        		float x = hitInfo.point.x;
	            float y = hitInfo.point.y;
	        	if(hitInfo.collider.tag == "Floor")
	        	{
                	isDown = true;
                	GameObject prefab = ((DrawGrid)target).m_obstaclePrefab;
                	ObstacleType type = ObstacleType.None;
                	if(prefab.GetComponent<Obstacle>() != null)
                	{
                		type = prefab.GetComponent<Obstacle>().obstacleType;
                	}
                	Vector3 initPoint = ((DrawGrid)target).EditMaze(hitInfo.point, type);
                	// Vector3 initPoint = ((DrawGrid)target).IdentifyCenter(hitInfo.point);
                	if(initPoint.x == -99)
                	{
                		return;
                	}
                	initPoint.y += prefab.GetComponent<BoxCollider>().size.y * prefab.transform.localScale.y / 2 * ((DrawGrid)target).m_gridSize;

					GameObject newobj = Instantiate(prefab, initPoint, Quaternion.Euler(0, 0, 0), ((DrawGrid)target).transform) as GameObject;  //设置障碍 
            		newobj.transform.localScale = ((DrawGrid)target).m_gridSize * newobj.transform.localScale;
            		Undo.RegisterCreatedObjectUndo(newobj, "NewObject");
	        	}
	        	else if(hitInfo.collider.tag == "Cube")
	        	{
		           	if (!isDown)
		            {
	                	isDown = true;
	                	Transform bottom = hitInfo.transform;
	                	DrawGrid drawgrid = bottom.gameObject.GetComponentInParent<DrawGrid>();
	                	GameObject prefab = drawgrid.m_obstaclePrefab;
	                	Vector3 initPoint = bottom.position;
	                	ObstacleType type = ObstacleType.None;
	                	if(prefab.GetComponent<Obstacle>() != null)
	                	{
	                		type = prefab.GetComponent<Obstacle>().obstacleType;
	                	}
	                	((DrawGrid)target).EditMaze(hitInfo.point, type);
	                	initPoint.y += bottom.gameObject.GetComponent<BoxCollider>().size.y * bottom.localScale.y / 2;
	                	if(prefab.GetComponent<BoxCollider>())
	                	{
		                	initPoint.y += prefab.GetComponent<BoxCollider>().size.y * prefab.transform.localScale.y / 2 * ((DrawGrid)target).m_gridSize;
	                	}

	                	GameObject newobj = Instantiate(prefab, initPoint, Quaternion.Euler(0, 0, 0), ((DrawGrid)target).transform) as GameObject;  //设置障碍 
                		newobj.transform.localScale = newobj.transform.localScale = ((DrawGrid)target).m_gridSize * newobj.transform.localScale;
                		Undo.RegisterCreatedObjectUndo(newobj, "NewObject");
		            }
	        	}
	        }
    	}
    	else if(action == ActionType.Destroy)
    	{
			if (Physics.Raycast(ray, out hitInfo, 2000, ~mask)) //这里设置层
	        {
	        	if(hitInfo.collider.tag == "Cube")
	        	{
					float x = hitInfo.point.x;
		            float y = hitInfo.point.y;
		            if (!isDown)
		            {
	                	isDown = true;
	                	Undo.DestroyObjectImmediate(hitInfo.transform.gameObject);
            	        RaycastHit newhitInfo;

	                	if(Physics.Raycast(ray, out newhitInfo, 2000, ~mask))
	                	{
	                		if(newhitInfo.collider.tag == "Floor")
	                		{
	                			((DrawGrid)target).EditorDeleteFromMaze(hitInfo.point, true);
	                			Debug.Log("lAST");
	                		}
	                		else
	                		{
	                			((DrawGrid)target).EditorDeleteFromMaze(hitInfo.point, false);
	                		}
	                	}
		            }
	        	}
	        }
    	}
    	else if(action == ActionType.Rotate)
    	{
    		if (Physics.Raycast(ray, out hitInfo, 2000, ~mask)) //这里设置层
	        {
	        	if(hitInfo.collider.tag == "Cube")
	        	{
					Transform cube = hitInfo.transform;
					cube.Rotate(0, 90, 0);
					/*Quaternion rotation = cube.rotation;
					rotation.y += 90;
					cube.rotation = rotation;*/
	        	}
	        }
    	}
    }
}


