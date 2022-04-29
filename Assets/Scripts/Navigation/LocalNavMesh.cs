using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-102)]
public class LocalNavMesh : MonoBehaviour
{
    NavMeshManager navMeshManager;

    private Vector3 size;
    private Vector3 position;

    void OnEnable()
    {
        position = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(position, transform.position) >= 5)
        {
            position = transform.position;
            navMeshManager.UpdateNavMesh(this);
        }
    }

    public Bounds GetBounds()
    {
        return new Bounds(position, size);
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
        return new Bounds(Quantize(transform.position, 0.1f * size), size);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = position;
        Gizmos.DrawWireCube(center, size);
    }

    public bool InBounds(Bounds bounds)
    {
        return GetBounds().Intersects(bounds);
    }

    public static class Factory
    {
        public static LocalNavMesh Create(Transform target, Vector3 size,NavMeshManager manager)
        {
            LocalNavMesh localNavMesh = target.gameObject.AddComponent<LocalNavMesh>();

            localNavMesh.size = size;
            localNavMesh.navMeshManager = manager;
            return localNavMesh;
        }
    }
}
