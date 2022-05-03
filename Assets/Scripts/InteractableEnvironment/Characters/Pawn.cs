using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class Pawn : MonoBehaviour
{

    [Zenject.Inject] private NavMeshManager manager;
    [SerializeField]
    private GameObject selectionCircle;

    private Animator animator;
    private NavMeshAgent agent;
    private NavDot navDot = new NavDot();

    private bool working = false;
    private Vector3 destination;

    public event EventHandler<Pawn> stopInteract;

    //-----------Object Controll-----------//

    private void OnEnable()
    {
        Physics.IgnoreLayerCollision(0, 7);

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            gameObject.AddComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        manager.AddLocalNavMesh(transform, new Vector3(10f, 20f, 10f));

        Deselect();
    }

    private void OnDisable()
    {
        manager.ReturnLocalNavMesh(GetComponent<LocalNavMesh>());
    }

    private void Update()
    {
        UpdateMovingAnimationState();
        UpdateWorkingAnimationState();
    }

    //-----------Player Orders-----------//

    public void MoveTo(Vector3 location)
    {
        navDot.SetNavMeshInstance(location);
        working = false;
        stopInteract?.Invoke(this, this);
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

    public void MoveTo(GameObject obj)
    {
        destination = obj.transform.position;
        working = false;
        stopInteract?.Invoke(this, this);

        Vector3 normal = Vector3.Normalize(new Vector3(transform.position.x - destination.x, 0, transform.position.z - destination.z));
        destination += new Vector3(normal.x + 0.1f, 0, normal.z + 0.1f);

        navDot.SetNavMeshInstance(destination);

        if (agent.SetDestination(destination))
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

    public void Interact(GameObject obj)
    {
        Interactable interact = obj.GetComponent<Interactable>();
        StopAllCoroutines();
        if (interact != null)
        {
            MoveTo(obj);
            working = true;
            StartCoroutine(IsStartInteract(interact, stopInteract));
        }
    }

    public void Select()
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(true);
    }

    public void Deselect()
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(false);
    }

    //-----------Animations-----------//

    private void UpdateMovingAnimationState()
    {
        if (agent.velocity.x != 0 || agent.velocity.z != 0) animator.SetBool("IsMoving", true);
        else animator.SetBool("IsMoving", false);
    }

    public void UpdateWorkingAnimationState()
    {
        if (working == true && Vector3.Distance(transform.position, destination) <= 1f)
            animator.SetBool("IsWorking", true);
        else animator.SetBool("IsWorking", false);
    }

    IEnumerator IsStartInteract(Interactable obj, EventHandler<Pawn> action)
    {
        while (working != false)
        {
            if (working == true && Vector3.Distance(transform.position, destination) <= 1f)
            {

                obj.StartInteraction(action);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
