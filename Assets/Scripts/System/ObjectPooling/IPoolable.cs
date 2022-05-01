using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable<T>
{
    void Initialize(System.Action<T> returnAction);
    void ReturnToPool();
}
