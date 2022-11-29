using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrassRenderer
{
    public class MeshLOD
    {
        public Mesh mesh;
        public Matrix4x4 meshMatrix;
        public ComputeBuffer argumentBuffer;
        public ComputeBuffer visibleGrassIDBuffer;
        public Material material;
    }


    bool m_ShouldBatchDispatch = true;

    MeshLOD[] m_MeshLODS;
    ComputeShader m_CullingComputeShader;
    List<GrassProvider.GrassTransform>[] m_CellTransformsLists;


    private ComputeBuffer m_GrassRotationBuffer;
    private ComputeBuffer m_GrassPositionBuffer;

    float m_MinX = float.MaxValue;
    float m_MinY = float.MaxValue;
    float m_MinZ = float.MaxValue;
    float m_MaxX = float.MinValue;
    float m_MaxY = float.MinValue;
    float m_MaxZ = float.MinValue;

    public GrassRenderer(GameObject GrassPrefab, int CellCount, ComputeShader cullingComputeShader)
    {
        m_CullingComputeShader = GameObject.Instantiate(cullingComputeShader);

        m_CellTransformsLists = new List<GrassProvider.GrassTransform>[CellCount];
        for (int i = 0; i < m_CellTransformsLists.Length; i++)
        {
            m_CellTransformsLists[i] = new List<GrassProvider.GrassTransform>();
        }

        var meshFilters = GrassPrefab.GetComponentsInChildren<MeshFilter>();
        m_MeshLODS = new MeshLOD[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            MeshLOD meshLOD = new MeshLOD();
            meshLOD.mesh = meshFilters[i].sharedMesh;
            meshLOD.meshMatrix = meshFilters[i].transform.localToWorldMatrix;
            meshLOD.material = new Material(meshFilters[i].GetComponent<Renderer>().sharedMaterial);
            meshLOD.material.EnableKeyword("_PROCEDURAL_INSTANCING_ON");


            m_MeshLODS[i] = meshLOD;
        }
    }

    public void AddCellTransform(int index, GrassProvider.GrassTransform grassTransform)
    {
        m_CellTransformsLists[index].Add(grassTransform);


        Vector3 target = grassTransform.position;
        m_MinX = Mathf.Min(target.x, m_MinX);
        m_MinY = Mathf.Min(target.y, m_MinY);
        m_MinZ = Mathf.Min(target.z, m_MinZ);
        m_MaxX = Mathf.Max(target.x, m_MaxX);
        m_MaxY = Mathf.Max(target.y, m_MaxY);
        m_MaxZ = Mathf.Max(target.z, m_MaxZ);
    }

    public void Draw(float drawDistance, List<int> visibleCellIDList,Camera cam)
    {
        Matrix4x4 v = cam.worldToCameraMatrix;
        Matrix4x4 p = cam.projectionMatrix;
        Matrix4x4 vp = p * v;

        for (int i = 0; i < m_MeshLODS.Length; i++)
        {
            var meshLOD = m_MeshLODS[i];
            meshLOD.visibleGrassIDBuffer.SetCounterValue(0);
        }

        m_CullingComputeShader.SetMatrix("_VPMatrix", vp);
        m_CullingComputeShader.SetFloat("_MaxDrawDistance", drawDistance);

        int dispatchCount = 0;
        for (int i = 0; i < visibleCellIDList.Count; i++)
        {
            int targetCellFlattenID = visibleCellIDList[i];
            int memoryOffset = 0;
            for (int j = 0; j < targetCellFlattenID; j++)
            {
                memoryOffset += m_CellTransformsLists[j].Count;
            }
            m_CullingComputeShader.SetInt("_StartOffset", memoryOffset);

            int jobLength = m_CellTransformsLists[targetCellFlattenID].Count;
            if (m_ShouldBatchDispatch)
            {
                while ((i < visibleCellIDList.Count - 1) && (visibleCellIDList[i + 1] == visibleCellIDList[i] + 1))
                {
                    jobLength += m_CellTransformsLists[visibleCellIDList[i + 1]].Count;
                    i++;
                }
            }

            if(jobLength>0)
                m_CullingComputeShader.Dispatch(0, Mathf.CeilToInt(jobLength / 64f), 1, 1);

            dispatchCount++;
        }


        Bounds renderBound = new Bounds();
        renderBound.SetMinMax(new Vector3(m_MinX, m_MinY, m_MinZ), new Vector3(m_MaxX, m_MaxY, m_MaxZ));

        for (int i = 0; i < m_MeshLODS.Length; i++)
        {
            var meshLOD = m_MeshLODS[i];

            ComputeBuffer.CopyCount(meshLOD.visibleGrassIDBuffer, meshLOD.argumentBuffer, 4);
            Graphics.DrawMeshInstancedIndirect(meshLOD.mesh, 0, meshLOD.material, renderBound, meshLOD.argumentBuffer, 0, null, UnityEngine.Rendering.ShadowCastingMode.Off, true, 0, cam, UnityEngine.Rendering.LightProbeUsage.Off);
        }
    }

    public void SetUpBuffers(int GrassCount)
    {
        if (m_GrassPositionBuffer != null)
            m_GrassPositionBuffer.Release();

        m_GrassPositionBuffer = new ComputeBuffer(GrassCount, sizeof(float) * 3);
        System.GC.SuppressFinalize(m_GrassPositionBuffer);

        if (m_GrassRotationBuffer != null)
            m_GrassRotationBuffer.Release();

        m_GrassRotationBuffer = new ComputeBuffer(GrassCount, sizeof(float) * 4);
        System.GC.SuppressFinalize(m_GrassRotationBuffer);

        int offset = 0;
        Vector3[] allGrassPosWSSortedByCell = new Vector3[GrassCount];
        Quaternion[] allGrassRotWSSortedByCell = new Quaternion[GrassCount];
        for (int i = 0; i < m_CellTransformsLists.Length; i++)
        {
            for (int j = 0; j < m_CellTransformsLists[i].Count; j++)
            {
                allGrassPosWSSortedByCell[offset] = m_CellTransformsLists[i][j].position;
                allGrassRotWSSortedByCell[offset] = m_CellTransformsLists[i][j].rotation;
                offset++;
            }
        }

        m_GrassPositionBuffer.SetData(allGrassPosWSSortedByCell);
        m_GrassRotationBuffer.SetData(allGrassRotWSSortedByCell);

        for (int i = 0; i < m_MeshLODS.Length; i++)
        {
            MeshLOD meshLOD = m_MeshLODS[i];

            if (meshLOD.visibleGrassIDBuffer != null)
                meshLOD.visibleGrassIDBuffer.Release();

            meshLOD.visibleGrassIDBuffer = new ComputeBuffer(GrassCount, sizeof(uint), ComputeBufferType.Append); //uint only, per visible grass
            System.GC.SuppressFinalize(meshLOD.visibleGrassIDBuffer);

            if (meshLOD.argumentBuffer != null)
                meshLOD.argumentBuffer.Release();

            uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
            meshLOD.argumentBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            System.GC.SuppressFinalize(meshLOD.argumentBuffer);

            args[0] = (uint)meshLOD.mesh.GetIndexCount(0);
            args[1] = (uint)GrassCount;
            args[2] = (uint)meshLOD.mesh.GetIndexStart(0);
            args[3] = (uint)meshLOD.mesh.GetBaseVertex(0);
            args[4] = 0;

            meshLOD.argumentBuffer.SetData(args);

            meshLOD.material.SetBuffer("rotationBuffer", m_GrassRotationBuffer);
            meshLOD.material.SetBuffer("positionBuffer", m_GrassPositionBuffer);
            meshLOD.material.SetBuffer("_VisibleInstanceOnlyTransformIDBuffer", meshLOD.visibleGrassIDBuffer);
            meshLOD.material.SetMatrix("_MeshMatrix", meshLOD.meshMatrix);
        }


        //set buffer
        m_CullingComputeShader.SetBuffer(0, "_AllInstancesPosWSBuffer", m_GrassPositionBuffer);
        m_CullingComputeShader.SetInt("_InstanceCount", m_GrassPositionBuffer.count);

        ComputeBuffer currentVisibleBuffer = null;
        for (int i = 0; i < 3; i++)
        {
            if (i < m_MeshLODS.Length)
                currentVisibleBuffer = m_MeshLODS[i].visibleGrassIDBuffer;

            m_CullingComputeShader.SetBuffer(0, "_VisibleGrassIDSLOD" + i.ToString(), currentVisibleBuffer);
        }
    }

    public void Dispose()
    {
        //release all compute buffers
        if (m_GrassPositionBuffer != null)
            m_GrassPositionBuffer.Release();
        m_GrassPositionBuffer = null;

        if (m_GrassRotationBuffer != null)
            m_GrassRotationBuffer.Release();
        m_GrassRotationBuffer = null;

        for (int i = 0; i < m_MeshLODS.Length; i++)
        {
            var meshLOD = m_MeshLODS[i];

            if (meshLOD.visibleGrassIDBuffer != null)
                meshLOD.visibleGrassIDBuffer.Release();
            meshLOD.visibleGrassIDBuffer = null;

            if (meshLOD.argumentBuffer != null)
                meshLOD.argumentBuffer.Release();
            meshLOD.argumentBuffer = null;
        }
    }
}
