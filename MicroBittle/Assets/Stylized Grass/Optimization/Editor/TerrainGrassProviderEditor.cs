using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TerrainGrassProvider))]
public class TerrainGrassProviderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.DrawDefaultInspector();

        TerrainGrassProvider terrainOptimization = (target as TerrainGrassProvider);


        if (Application.isPlaying)
            GUI.enabled = false;

        if (!terrainOptimization.IsOptimized())
        {
            if (GUILayout.Button("Convert To Optimized Grass"))
            {
                terrainOptimization.ConvertToGrassTransforms();
            }
        }
        else
        {
            if (GUILayout.Button("Convert Back To Terrain Grass"))
            {
                terrainOptimization.ConvertToTerrainTrees();
            }
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif