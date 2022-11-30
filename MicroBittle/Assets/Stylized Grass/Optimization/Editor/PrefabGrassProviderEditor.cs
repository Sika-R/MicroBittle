using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PrefabGrassProvider))]
public class PrefabGrassProviderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();

        PrefabGrassProvider prefabOptimization = (target as PrefabGrassProvider);

        if (Application.isPlaying)
            GUI.enabled = false;

        if (!prefabOptimization.IsOptimized())
        {
            if (GUILayout.Button("Convert To Optimized Grass"))
            {
                prefabOptimization.ConvertToGrassTransforms();
            }
        }
        else
        {
            if (GUILayout.Button("Convert Back To GameObjects"))
            {
                prefabOptimization.ConvertToGameObjects();
            }
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif