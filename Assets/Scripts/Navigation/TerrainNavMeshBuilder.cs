using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshSurface))]
public class TerrainNavMeshBuilder : MonoBehaviour
{
    private NavMeshSurface surface;

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        surface.collectObjects = CollectObjects.Children;
    }

    public void UpdateNavMesh()
    {
        surface.collectObjects = CollectObjects.Children;
        surface.RemoveData();
        surface.BuildNavMesh();
    }
}
