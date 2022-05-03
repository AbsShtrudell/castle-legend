using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private Action<T> pullObject;
    private Action<T> pushObject;
    private Stack<T> pooledObject = new Stack<T>();
    private GameObject prefab;
    public int pooledCount
    {
        get { return pooledObject.Count; }
    }

    public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        this.pullObject = pullObject;
        this.pushObject = pushObject;
        Spawn(numToSpawn);
    }

    public ObjectPool(GameObject pooledObject, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        Spawn(numToSpawn);
    }

    private void Spawn(int numToSpawn)
    {
        T obj;
        for(int i = 0; i < numToSpawn; i++)
        {
            obj = GameObject.Instantiate(prefab).GetComponent<T>();
            pooledObject.Push(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public T Pull()
    {
        T obj;
        if(pooledCount > 0)
            obj = pooledObject.Pop();
        else
            obj = GameObject.Instantiate(prefab).GetComponent<T>();

        obj.gameObject.SetActive(true);
        obj.Initialize(Push);

        pullObject?.Invoke(obj);

        return obj;
    }

    public void Push(T obj)
    {
        if (!pooledObject.Contains(obj))
        {
            pooledObject.Push(obj);

            pushObject?.Invoke(obj);

            obj.gameObject.SetActive(false);
        }
    }
}

