using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField]
    List<Material> allMats = new List<Material>();
    [SerializeField]
    List<Mesh> allMeshes = new List<Mesh>();
    Renderer r;
    MeshFilter meshFilter;
    [SerializeField]
    Biome curBiome;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();
        ChangeMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeMaterial()
    {
        if (CreativeMgr.Instance)
        {
            /*switch (CreativeMgr.Instance.curBiome)
            {
                case (Biome.desert):
                    renderer.material = allMats[0]
                    break;
            }*/
            r.material = allMats[(int)CreativeMgr.Instance.curBiome];
            meshFilter.sharedMesh = allMeshes[(int)CreativeMgr.Instance.curBiome];
        }
        else
        {
            r.material = allMats[(int)curBiome];
            meshFilter.sharedMesh = allMeshes[(int)curBiome];
        }
    }
}
