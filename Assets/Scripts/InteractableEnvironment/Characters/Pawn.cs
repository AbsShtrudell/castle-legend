using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn : MonoBehaviour
{
    public GameObject manager;

    void Start()
    {
        Physics.IgnoreLayerCollision(0, 7);

        manager.GetComponent<NavMeshManager>().CreateLocalNavMesh(transform, new Vector3(20f, 11f, 20f));
    }
}
