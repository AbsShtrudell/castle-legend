using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn : MonoBehaviour
{
    [Zenject.Inject]private NavMeshManager manager;

    void Start()
    {
        Physics.IgnoreLayerCollision(0, 7);

        manager.AddLocalNavMesh(transform, new Vector3(20f, 11f, 20f));
    }
}
