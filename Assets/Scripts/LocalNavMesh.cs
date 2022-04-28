using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

[DefaultExecutionOrder(-102)]
public class LocalNavMesh : MonoBehaviour
{
    public Vector3 m_Size = new Vector3(80.0f, 20.0f, 80.0f);

    Vector3 m_Position;
    public NavMeshData m_NavMesh;
    AsyncOperation m_Operation;
    public NavMeshDataInstance m_Instance;
    NavMeshAgent m_Agent;
    public NavMeshManager navMeshManager;
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

    void OnEnable()
    {
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        m_Position = transform.position;
    }

    private void Start()
    {
    }

    void OnDisable()
    {
        m_Instance.Remove();
    }

    private void Update()
    {
        if (Vector3.Distance(m_Position, transform.position) >= 5)
        {
            m_Position = transform.position;
            navMeshManager.UpdateNavMesh(this);
        }
    }

    public void UpdateNavMesh(Bounds bounds, bool asyncUpdate = false)
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        if (asyncUpdate)
            m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
    }

    public Bounds GetBounds()
    {
        return new Bounds(m_Position, m_Size);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        return new Bounds(Quantize(transform.position, 0.1f * m_Size), m_Size);
    }

    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = m_Position;
        Gizmos.DrawWireCube(center, m_Size);
    }

    public bool InBounds(Bounds bounds)
    {
        Bounds localBounds = GetBounds();
        for (int i = 0; i < Cube.CornerTableCenter.Length; i++)
        {
            Vector3 point = new Vector3(Cube.CornerTableCenter[i].x * bounds.size.x, Cube.CornerTableCenter[i].y * bounds.size.y, Cube.CornerTableCenter[i].z * bounds.size.z) + bounds.center;
            if(localBounds.Contains(point)) return true;
        }
        return false;
    }
}
