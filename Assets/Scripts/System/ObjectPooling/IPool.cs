using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool<T>
{
    T Pull();
    void Push(T obj);
}
