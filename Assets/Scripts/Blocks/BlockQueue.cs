using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockQueue: MonoBehaviour
{
    public abstract IBlock GetBlock(Collider collider);
}
