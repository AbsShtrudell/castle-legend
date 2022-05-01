using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pawn : MonoBehaviour
{
    [Zenject.Inject]private NavMeshManager manager;

    private Animator animator;
    private NavMeshAgent agent;

    public event Action reachedGoal;

    void Start()
    {
        Init();
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        UpdateMovingAnimationState();
    }

    public void Init()
    {
        Physics.IgnoreLayerCollision(0, 7);

        agent = GetComponent<NavMeshAgent>();
        if(agent == null)
            gameObject.AddComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        manager.AddLocalNavMesh(transform, new Vector3(10f, 20f, 10f));
    }

    public void MoveTo(Vector3 location)
    {
        if (agent.SetDestination(location))
        {
            agent.isStopped = false;
            animator.SetTrigger("StartMoving");
        }
        else
        {
            animator.ResetTrigger("StartMoving");
            animator.SetTrigger("Confused");
            agent.isStopped = true;
        }
    }

    private void UpdateMovingAnimationState()
    {
        if (agent.velocity.x != 0 || agent.velocity.z != 0) animator.SetBool("IsMoving", true);
        else animator.SetBool("IsMoving", false);
    }
}
