using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrassProvider:MonoBehaviour
{
    [System.Serializable]
    public struct GrassTransform
    {
        public Vector3 position;
        public Quaternion rotation;

        public GrassTransform(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
    
    [System.Serializable]
    public struct GrassCollection {
        public GameObject GrassPrefab;
        public List<GrassTransform> GrassTransforms;

        public GrassCollection(GameObject grassPrefab, List<GrassTransform> grassTransforms)
        {
            GrassPrefab = grassPrefab;
            GrassTransforms = grassTransforms;
        }
    }

    protected void AddGrassCollections(GrassCollection[] grassCollection) {
        //
        GlobalGrassRenderer.INSTANCE.RemoveGrassCollections(this, false);
        GlobalGrassRenderer.INSTANCE.AddGrassCollections(this,grassCollection);
    }
    protected void RemoveGrassCollections() {
        GlobalGrassRenderer.INSTANCE.RemoveGrassCollections(this,true);
    }

    protected GrassCollection[] GetGrassCollections()
    {
        return GlobalGrassRenderer.INSTANCE.GetGrassCollections(this);
    }
}
