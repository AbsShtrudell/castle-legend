using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable<PoolObject>
{
    private Action<PoolObject> returnToPool;

    private void OnDisable()
    {
        ReturnToPool();
    }

    public void Initialize(Action<PoolObject> returnAction)
    {
        this.returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }
}
