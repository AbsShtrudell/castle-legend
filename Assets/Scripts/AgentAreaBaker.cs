using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAreaBaker : MonoBehaviour
{

    public GameObject terrain;
    private NavMeshSurface surface;

    void Start()
    {
        surface = terrain.GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, surface.center) >= 5)
            UpdateNavMesh();
    }

    void UpdateNavMesh()
    {
        surface.collectObjects = CollectObjects.Children;
        //surface.center = transform.position;
        //surface.BuildNavMesh();
        surface.AddData();
        surface.UpdateNavMesh(surface.navMeshData);
    }
}
