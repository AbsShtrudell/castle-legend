using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class StaticBlockQueue : BlockQueue
{
    private Chunk chunk;

    private void OnEnable()
    {
        chunk = GetComponent<Chunk>();
    }

    private void OnDisable()
    {
        chunk = null;
    }

    public override IBlock GetBlock(Collider collider)
    {
        return chunk.GetBlock((BoxCollider)collider);
    }
}
