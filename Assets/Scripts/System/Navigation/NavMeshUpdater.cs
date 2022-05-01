using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUpdater
{
    public static void UpdateNavMesh(Bounds bounds, NavMeshData navMeshData, bool asyncUpdate = false)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuildSettings defaultBuildSettings = NavMesh.GetSettingsByID(0);

        NavMeshSourceTag.Collect(ref sources);

        if (asyncUpdate)
            NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, defaultBuildSettings, sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(navMeshData, defaultBuildSettings, sources, bounds);
    }
}
