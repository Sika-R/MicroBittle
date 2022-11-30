using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(GlobalGrassRenderer))]
public class GlobalGrassRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();

        GlobalGrassRenderer globalGrassRenderer = (target as GlobalGrassRenderer);

        if (GUILayout.Button("Force Refresh"))
        {
            globalGrassRenderer.ForceRefresh();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif