using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable
{ 
    public void StartInteraction(EventHandler<Pawn> stopAction);
    public void StopInteraction(object sender, Pawn pawn);
}
