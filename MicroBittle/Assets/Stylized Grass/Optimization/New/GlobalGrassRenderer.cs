using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
#endif

[ExecuteAlways]
public class GlobalGrassRenderer : MonoBehaviour
{
    [System.Serializable]
    struct GrassCollectionByOwner {
        public MonoBehaviour owner;
        public GrassProvider.GrassCollection[] grassCollections;

        public GrassCollectionByOwner(MonoBehaviour owner, GrassProvider.GrassCollection[] grassCollections)
        {
            this.owner = owner;
            this.grassCollections = grassCollections;
        }
    }

    public float drawDistance = 125;

    [Header("Advanced")]
    [SerializeField]float m_CellSize = 10;
    [SerializeField] float m_MinimumCPUCullingTime = 0.1f;

    public ComputeShader cullingComputeShader;

    [SerializeField] [HideInInspector]
    List<GrassCollectionByOwner> m_GrassCollectionsByOwners = new List<GrassCollectionByOwner>();

    Dictionary<GameObject, List<GrassProvider.GrassTransform>> m_FinalGrassCollections = new Dictionary<GameObject, List<GrassProvider.GrassTransform>>();

    static GlobalGrassRenderer _instance;
    public static GlobalGrassRenderer INSTANCE
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalGrassRenderer>();
            }
            if (_instance == null)
            {
                UnityEngine.Debug.LogError("No Global Grass Renderer found! Please drag one in the scene! (Stylized Grass/GlobalGrassRenderer)");
            }

            return _instance;
        }
    }

    private int m_CellCountX = -1;
    private int m_CellCountY = -1;
    private int m_CellCountZ = -1;

    private float m_CellSizeX = 10;
    private float m_CellSizeY = 10;
    private float m_CellSizeZ = 10;

    private List<GrassProvider.GrassTransform>[] m_CellTransformsLists;

    private float m_MinX, m_MinZ,m_MinY,m_MaxY, m_MaxX, m_MaxZ;
    private List<int> m_VisibleCellIDList = new List<int>();
    private Plane[] m_CameraFrustumPlanes = new Plane[6];

    GrassRenderer[] m_GrassRenderers;

    private float m_LastCPUCullUpdate = 0f;

    private void OnEnable()
    {
#if !USING_HDRP && !USING_URP
        Camera.onPreCull -= Draw;
        Camera.onPreCull += Draw;
#else
        if (GraphicsSettings.currentRenderPipeline == null)
        {
            Camera.onPreCull -= Draw;
            Camera.onPreCull += Draw;
        }
        else
        {
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
        }
#endif

        FinalizeGrassCollection();
#if UNITY_EDITOR
        EditorSceneManager.sceneSaved += EditorSceneManager_sceneSaved;
#endif
        InitializeBuffersAndTransforms(false);
    }

    private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera cam)
    {
        Draw(cam);
    }


#if UNITY_EDITOR
    private void EditorSceneManager_sceneSaved(UnityEngine.SceneManagement.Scene scene)
    {
        ForceRefresh(false); 
    }
#endif

    void FinalizeGrassCollection() {
        m_GrassCollectionsByOwners = m_GrassCollectionsByOwners.Where(x => x.owner != null).ToList();

        m_FinalGrassCollections.Clear();
        //Finalize all grass collections
        foreach (var grasscollectionbyowner in m_GrassCollectionsByOwners)
        {
            foreach (var grasscollection in grasscollectionbyowner.grassCollections)
            {
                if (!m_FinalGrassCollections.ContainsKey(grasscollection.GrassPrefab))
                    m_FinalGrassCollections.Add(grasscollection.GrassPrefab, new List<GrassProvider.GrassTransform>());

                var List = m_FinalGrassCollections[grasscollection.GrassPrefab];
                List.AddRange(grasscollection.GrassTransforms);
            }
        }
    }

    public void ForceRefresh(bool logInfo=true) {
        //Reset the buffers on the materials on save (Unity clears the instantiated material on save)
        Dispose();

        FinalizeGrassCollection();
        InitializeBuffersAndTransforms(logInfo);
    }

    void Draw(Camera cam)
    {
        if (cam.cameraType == CameraType.Preview || cam.cameraType == CameraType.Reflection)
            return;

#if UNITY_EDITOR
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            return;
#endif

        if (m_GrassRenderers.Length == 0)
            return;

#if UNITY_EDITOR
        if(!Application.isPlaying)
            m_LastCPUCullUpdate = 0f;
#endif
        if (Time.time - m_LastCPUCullUpdate > m_MinimumCPUCullingTime)
        {
            m_LastCPUCullUpdate = Time.time;
            m_VisibleCellIDList.Clear();

            float cameraOriginalFarPlane = cam.farClipPlane;
            cam.farClipPlane = drawDistance;
            GeometryUtility.CalculateFrustumPlanes(cam, m_CameraFrustumPlanes);
            cam.farClipPlane = cameraOriginalFarPlane;

            for (int i = 0; i < m_CellTransformsLists.Length; i++)
            {
                if (m_CellTransformsLists[i].Count == 0)
                    continue;

                int id = i;
                int z = id / (int)(m_CellCountX * m_CellCountY);
                id -= (z * m_CellCountX * m_CellCountY);
                int y = id / m_CellCountX;
                int x = id % m_CellCountX;

                Vector3 centerPosWS = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);

                centerPosWS.x = Mathf.Lerp(m_MinX, m_MaxX, centerPosWS.x / m_CellCountX);
                centerPosWS.y = Mathf.Lerp(m_MinY, m_MaxY, centerPosWS.y / m_CellCountY);
                centerPosWS.z = Mathf.Lerp(m_MinZ, m_MaxZ, centerPosWS.z / m_CellCountZ);

                Vector3 sizeWS = new Vector3(Mathf.Abs(m_MaxX - m_MinX) / m_CellCountX, Mathf.Abs(m_MaxY - m_MinY) / m_CellCountY, Mathf.Abs(m_MaxZ - m_MinZ) / m_CellCountZ);
                Bounds cellBound = new Bounds(centerPosWS, sizeWS);

                if (GeometryUtility.TestPlanesAABB(m_CameraFrustumPlanes, cellBound))
                {
                    m_VisibleCellIDList.Add(i);
                }
            }
        }

        foreach (var grassRenderer in m_GrassRenderers)
        {
            grassRenderer.Draw(drawDistance, m_VisibleCellIDList,cam);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (int index in m_VisibleCellIDList)
        {
            int id = index;
            int z = id / (int)(m_CellCountX * m_CellCountY);
            id -= (z * m_CellCountX * m_CellCountY);
            int y = id / m_CellCountX;
            int x = id % m_CellCountX;

            Vector3 centerPosWS = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
            centerPosWS.x = Mathf.Lerp(m_MinX, m_MaxX, centerPosWS.x / m_CellCountX);
            centerPosWS.y = Mathf.Lerp(m_MinY, m_MaxY, centerPosWS.y / m_CellCountY);
            centerPosWS.z = Mathf.Lerp(m_MinZ, m_MaxZ, centerPosWS.z / m_CellCountZ);
            Vector3 sizeWS = new Vector3(Mathf.Abs(m_MaxX - m_MinX) / m_CellCountX, Mathf.Abs(m_MaxY - m_MinY)/m_CellCountY, Mathf.Abs(m_MaxZ - m_MinZ) / m_CellCountZ);
            Bounds cellBound = new Bounds(centerPosWS, sizeWS);

            Gizmos.DrawWireCube(cellBound.center, cellBound.size);
        }
    }

    public void AddGrassCollections(MonoBehaviour owner, GrassProvider.GrassCollection[] grassCollections)
    {
        m_GrassCollectionsByOwners.Add(new GrassCollectionByOwner(owner, grassCollections));

        ForceRefresh();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public void RemoveGrassCollections(MonoBehaviour owner,bool log)
    {
        m_GrassCollectionsByOwners.RemoveAll(item => item.owner == owner);

        ForceRefresh(log);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public GrassProvider.GrassCollection[] GetGrassCollections(MonoBehaviour owner)
    {
        var collections=m_GrassCollectionsByOwners.FindAll(item => item.owner == owner);

        List<GrassProvider.GrassCollection> grassCollections = new List<GrassProvider.GrassCollection>();
        foreach (var collection in collections)
            grassCollections.AddRange(collection.grassCollections);

        return grassCollections.ToArray();
    }

    void OnDisable()
    {
        Dispose();
        Camera.onPreCull -= Draw;
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
        _instance = null;
    }

    void Dispose() {
        if(m_GrassRenderers!=null)
        foreach (var GrassRenderer in m_GrassRenderers)
        {
            GrassRenderer.Dispose();
        }
    }

    void InitializeBuffersAndTransforms(bool logInfo=true)
    {
        m_CellSizeX = m_CellSize;
        m_CellSizeY = m_CellSize;
        m_CellSizeZ = m_CellSize;

        m_GrassRenderers = new GrassRenderer[0];

        if (m_FinalGrassCollections.Count == 0)
            return;

        m_MinX = float.MaxValue;
        m_MinZ = float.MaxValue;
        m_MinY = float.MaxValue;
        m_MaxX = float.MinValue;
        m_MaxY = float.MinValue;
        m_MaxZ = float.MinValue;

        foreach (var GrassCollection in m_FinalGrassCollections)
        {
            var Collection = GrassCollection.Value;

            for (int i = 0; i < Collection.Count; i++)
            {
                Vector3 target = Collection[i].position;
                m_MinX = Mathf.Min(target.x, m_MinX);
                m_MinY = Mathf.Min(target.y, m_MinY);
                m_MinZ = Mathf.Min(target.z, m_MinZ);
                m_MaxX = Mathf.Max(target.x, m_MaxX);
                m_MaxY = Mathf.Max(target.y + GrassCollection.Key.GetComponentInChildren<Renderer>().bounds.size.y,m_MaxY);
                m_MaxZ = Mathf.Max(target.z, m_MaxZ);
            }
        }

        if (m_MinX==float.MaxValue)
            return;

        m_CellCountX = Mathf.Max(1,Mathf.CeilToInt((m_MaxX - m_MinX) / m_CellSizeX));
        m_CellCountY = Mathf.Max(1, Mathf.CeilToInt((m_MaxY - m_MinY) / m_CellSizeY));
        m_CellCountZ = Mathf.Max(1,Mathf.CeilToInt((m_MaxZ - m_MinZ) / m_CellSizeZ));

        if (logInfo)
        {
            UnityEngine.Debug.Log("Max Grass Position: " + (int)m_MaxX + "," + (int)m_MaxZ + ". Min Grass Position: " + (int)m_MinX + "," + (int)m_MinZ);
            UnityEngine.Debug.Log("Generating grid with size " + m_CellCountX + "x" + m_CellCountY+"x"+m_CellCountZ);
        }

        m_CellTransformsLists = new List<GrassProvider.GrassTransform>[m_CellCountX * m_CellCountY* m_CellCountZ];
        for (int i = 0; i < m_CellTransformsLists.Length; i++)
        {
            m_CellTransformsLists[i] = new List<GrassProvider.GrassTransform>();
        }


        m_GrassRenderers = new GrassRenderer[m_FinalGrassCollections.Count];

        int counter = 0;
        foreach (var GrassCollection in m_FinalGrassCollections)
        {
            var Collection = GrassCollection.Value;

            m_GrassRenderers[counter] = new GrassRenderer(GrassCollection.Key, m_CellCountX * m_CellCountY* m_CellCountZ, cullingComputeShader);

            for (int i = 0; i < Collection.Count; i++)
            {
                Vector3 pos = Collection[i].position;

                int xID = Mathf.Min(m_CellCountX - 1, Mathf.FloorToInt(Mathf.InverseLerp(m_MinX, m_MaxX, pos.x) * m_CellCountX));
                int yID = Mathf.Min(m_CellCountX - 1, Mathf.FloorToInt(Mathf.InverseLerp(m_MinY, m_MaxY, pos.y) * m_CellCountY));
                int zID = Mathf.Min(m_CellCountZ - 1, Mathf.FloorToInt(Mathf.InverseLerp(m_MinZ, m_MaxZ, pos.z) * m_CellCountZ));

                int cellID = (zID * m_CellCountX * m_CellCountY) + (yID * m_CellCountX) + xID;

                m_CellTransformsLists[cellID].Add(Collection[i]);

                m_GrassRenderers[counter].AddCellTransform(cellID, Collection[i]);
            }

            m_GrassRenderers[counter].SetUpBuffers(Collection.Count);
            counter++;
        }
    }
}