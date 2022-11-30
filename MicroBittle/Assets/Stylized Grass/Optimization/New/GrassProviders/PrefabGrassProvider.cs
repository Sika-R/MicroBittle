using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabGrassProvider : GrassProvider
{
    [System.Serializable]
    class ListTransformWrapper<T> //We need a wrapper since Unity doesn't support serializing lists of lists
    {
        public List<T> transforms=new List<T>();
    }

    [SerializeField][HideInInspector]
    SerializableDictionary<GameObject, ListTransformWrapper<GrassProvider.GrassTransform>> m_GrassCollections = new SerializableDictionary<GameObject, ListTransformWrapper<GrassTransform>>();

    [SerializeField] [HideInInspector] bool m_IsOptimized = false;
    public void ConvertToGameObjects()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {

            foreach (var grassprefab in m_GrassCollections)
            {
                foreach (var grasstransform in grassprefab.Value.transforms)
                {
                    var go = PrefabUtility.InstantiatePrefab(grassprefab.Key, transform) as GameObject;
                    go.transform.position = grasstransform.position;
                    go.transform.rotation = grasstransform.rotation;
                    go.transform.localScale = new Vector3 (go.transform.localScale.x/transform.lossyScale.x, go.transform.localScale.y/transform.lossyScale.y, go.transform.localScale.z/transform.lossyScale.z);
                }
            }
            m_GrassCollections.Clear();

            RemoveGrassCollections();
            m_IsOptimized = false;
        }
#endif
    }

    public void ConvertToGrassTransforms()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var grassprefabs = GetComponentsInChildren<GrassPrefab>();

            foreach (var grassprefab in grassprefabs)
            {
                GameObject originalprefab = PrefabUtility.GetCorrespondingObjectFromSource(grassprefab.gameObject);

                if (!m_GrassCollections.ContainsKey(originalprefab))
                    m_GrassCollections.Add(originalprefab, new ListTransformWrapper<GrassTransform>());

                m_GrassCollections[originalprefab].transforms.Add(new GrassTransform(grassprefab.transform.position, grassprefab.transform.rotation));

                DestroyImmediate(grassprefab.gameObject);
            }

            SetGrassCollections();
            m_IsOptimized = true;
        }
#endif
    }

    public void SetGrassCollections()
    {
        List<GrassProvider.GrassCollection> GrassCollections = new List<GrassProvider.GrassCollection>();
        foreach (var grass in m_GrassCollections)
        {
            GrassCollections.Add(new GrassProvider.GrassCollection(grass.Key, grass.Value.transforms));
        }

        AddGrassCollections(GrassCollections.ToArray());
    }

    public bool IsOptimized()
    {
        return m_IsOptimized;
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) //Was Deleted
        {
            RemoveGrassCollections();
        }
    }
}
