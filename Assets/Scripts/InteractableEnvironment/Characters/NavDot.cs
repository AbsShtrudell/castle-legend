using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavDot
{
    private NavMeshDataInstance instance;

    public void SetNavMeshInstance(Vector3 location)
    {
        Destroy();

        NavMeshData nav = new NavMeshData();
        instance = NavMesh.AddNavMeshData(nav);

        NavMeshUpdater.UpdateNavMesh(new Bounds(location, Vector3.one * 0.1f), nav);
    }

    private void Destroy()
    {
        NavMesh.RemoveNavMeshData(instance);
    }
}
