using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicBlock))]
public class DynamicBlockQueue : BlockQueue
{
    private DynamicBlock block;

    private void OnEnable()
    {
        block = GetComponent<DynamicBlock>();
    }

    private void OnDisable()
    {
        block = null;
    }

    public override IBlock GetBlock(Collider collider)
    {
        return block;
    }
}
