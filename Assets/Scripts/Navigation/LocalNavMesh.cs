using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-102)]
public class LocalNavMesh : MonoBehaviour
{
    NavMeshManager navMeshManager;

    private Bounds bounds;
    private float UpdateDistance;

    void OnEnable()
    {
        bounds.center = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(bounds.center, transform.position) >= UpdateDistance)
        {
            bounds.center = transform.position;
            navMeshManager.UpdateNavMesh(this);
        }
    }

    public Bounds GetBounds()
    {
        return bounds;
    }

    public bool InBounds(Bounds bounds)
    {
        return GetBounds().Intersects(bounds);
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
        return new Bounds(Quantize(transform.position, 0.1f * bounds.size), bounds.size);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = bounds.center;
        Gizmos.DrawWireCube(center, bounds.size);
    }

    public static class Factory
    {
        public static LocalNavMesh Create(Transform target, Vector3 size,NavMeshManager manager)
        {
            LocalNavMesh localNavMesh = target.gameObject.AddComponent<LocalNavMesh>();

            localNavMesh.bounds = new Bounds(target.position, size);

            localNavMesh.UpdateDistance = localNavMesh.bounds.size.x >= localNavMesh.bounds.size.z ? 
                localNavMesh.bounds.size.z / 3 : localNavMesh.bounds.size.x / 3;

            localNavMesh.navMeshManager = manager;
            return localNavMesh;
        }
    }
}
