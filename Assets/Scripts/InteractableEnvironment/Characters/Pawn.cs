using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pawn : MonoBehaviour
{
    [Zenject.Inject]private NavMeshManager manager;

    private NavMeshAgent agent;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        Physics.IgnoreLayerCollision(0, 7);

        agent = GetComponent<NavMeshAgent>();
        if(agent == null)
            gameObject.AddComponent<NavMeshAgent>();

        manager.AddLocalNavMesh(transform, new Vector3(10f, 20f, 10f));
    }

    public void MoveTo(Vector3 location)
    {
        agent.Move(location);
    }
}
