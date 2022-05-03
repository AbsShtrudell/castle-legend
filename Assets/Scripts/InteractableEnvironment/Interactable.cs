using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Zenject.Inject]
    private PlayerController player;
    [SerializeField]
    private float resources = 1;
    [SerializeField]
    private float delay = 2;
    bool working = false;

    List<Coroutine> coroutines = new List<Coroutine>();

    private void OnEnable()
    {
        
    }

    public void StartInteraction(EventHandler<Pawn> stopAction)
    {
        stopAction = StopInteraction;
        working = true;
        Coroutine coroutine = StartCoroutine(UpdateInteraction());
        coroutines.Add(coroutine);
    }

    public void StopInteraction(object sender, Pawn pawn)
    {
        if (coroutines.Count > 0)
        {
            StopCoroutine(coroutines[coroutines.Count - 1]);
            coroutines.RemoveAt(coroutines.Count - 1);
            pawn.stopInteract -= StopInteraction;
        }
    }

    IEnumerator UpdateInteraction()
    {
        while (working)
        {
            player.AddResources(resources);
            yield return new WaitForSeconds(delay);
        }
    }
}
