using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
public class TerrainGrassProvider : GrassProvider
{
    [SerializeField]
    List<GameObject> m_GrassToOptimize;

    Dictionary<GameObject, List<GrassProvider.GrassTransform>> m_GrassCollections = new Dictionary<GameObject, List<GrassProvider.GrassTransform>>();

    [SerializeField] [HideInInspector] bool m_IsOptimized = false;
    public void ConvertToTerrainTrees()
    {
        if (!Application.isPlaying)
        {
            m_GrassCollections.Clear();
            var grasscollections = GetGrassCollections();

            TerrainData terrain = GetComponent<Terrain>().terrainData;

            Vector3 TerrainOffset = transform.position;

            List<TreeInstance> treeInstances = new List<TreeInstance>();
            Dictionary<GameObject, int> treeGameObjects = new Dictionary<GameObject, int>();

            foreach (var grasscollection in grasscollections)
            {
                if (!treeGameObjects.ContainsKey(grasscollection.GrassPrefab))
                {
                    treeGameObjects.Add(grasscollection.GrassPrefab, treeGameObjects.Count);
                }
                int index = treeGameObjects[grasscollection.GrassPrefab];

                var transforms = grasscollection.GrassTransforms;
                foreach (var transform in transforms)
                {
                    Vector3 position = transform.position;
                    position -= TerrainOffset;

                    // Extract new local rotation
                    Quaternion rotation = transform.rotation;

                    // Extract new local scale
                    Vector3 scale = Vector3.one;

                    TreeInstance treeInstance = new TreeInstance();
                    treeInstance.position = new Vector3(position.x / terrain.size.x, position.y / terrain.size.y, position.z / terrain.size.z);
                    treeInstance.rotation = rotation.eulerAngles.y;
                    treeInstance.heightScale = scale.y;
                    treeInstance.widthScale = scale.x;
                    treeInstance.prototypeIndex = index + terrain.treePrototypes.Length;

                    treeInstances.Add(treeInstance);
                }
            }

            TreePrototype[] treeprototypes = new TreePrototype[treeGameObjects.Count];
            foreach (var gameobjecttree in treeGameObjects)
            {
                TreePrototype treePrototype = new TreePrototype();
                treePrototype.prefab = gameobjecttree.Key;

                treeprototypes[gameobjecttree.Value] = treePrototype;
            }

            terrain.treePrototypes = terrain.treePrototypes.Concat(treeprototypes).ToArray();
            terrain.treeInstances = terrain.treeInstances.Concat(treeInstances).ToArray();

            RemoveGrassCollections();
            m_IsOptimized = false;
        }
    }

    public void ConvertToGrassTransforms()
    {
        if (!Application.isPlaying)
        {
            TerrainData terrain = GetComponent<Terrain>().terrainData;

            foreach (var treeinstance in terrain.treeInstances)
            {
                var Prefab = terrain.treePrototypes[treeinstance.prototypeIndex].prefab;
                if (m_GrassToOptimize.Contains(Prefab))
                {
                    Vector3 worldPosition = new Vector3(treeinstance.position.x * terrain.size.x, treeinstance.position.y * terrain.size.y, treeinstance.position.z * terrain.size.z) + transform.position;
                    Quaternion rotation = Quaternion.Euler(0f, treeinstance.rotation, 0f);


                    if (!m_GrassCollections.ContainsKey(Prefab))
                    {
                        m_GrassCollections.Add(Prefab, new List<GrassProvider.GrassTransform>());

                    }

                    List<GrassProvider.GrassTransform> grassTransforms = m_GrassCollections[Prefab];
                    grassTransforms.Add(new GrassProvider.GrassTransform(worldPosition, rotation));
                }
            }

            var newTreePrototypes = terrain.treePrototypes.Where(x => (!m_GrassToOptimize.Contains(x.prefab))).ToArray();
            Dictionary<GameObject, int> RemappedFoliage = new Dictionary<GameObject, int>();

            for (int i = 0; i < newTreePrototypes.Length; i++)
                RemappedFoliage.Add(newTreePrototypes[i].prefab, i);

            var newTreeInstances = terrain.treeInstances.Where(x => (!m_GrassToOptimize.Contains(terrain.treePrototypes[x.prototypeIndex].prefab))).ToArray();

            for (int i = 0; i < newTreeInstances.Length; i++)
            {
                newTreeInstances[i].prototypeIndex = RemappedFoliage[terrain.treePrototypes[newTreeInstances[i].prototypeIndex].prefab];
            }

            terrain.treeInstances = newTreeInstances;
            terrain.treePrototypes = newTreePrototypes;

            SetGrassCollections();
            m_IsOptimized = true;
        }
    }

    public void SetGrassCollections()
    {
        List<GrassProvider.GrassCollection> GrassCollections = new List<GrassProvider.GrassCollection>();
        foreach (var grass in m_GrassCollections)
        {
            GrassCollections.Add(new GrassProvider.GrassCollection(grass.Key, grass.Value));
        }

        AddGrassCollections(GrassCollections.ToArray());
    }

    public bool IsOptimized() {
        return m_IsOptimized;
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) //Was Deleted
        {
            ConvertToTerrainTrees();
        }
    }
}
