using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This is an interface for the object pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    #region fields
    private Action<T> pullObject;
    private Action<T> pushObject;
    private Stack<T> pooledObjects = new Stack<T>();
    private GameObject prefab;
    #endregion

    /// <summary>
    /// Constructor for the object pool.
    /// </summary>
    /// <param name="pooledObject"></param>
    /// <param name="numToSpawn"></param>
    public ObjectPool(GameObject pooledObject, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        Spawn(numToSpawn);
    }

    /// <summary>
    /// Constructor for the object pool.
    /// </summary>
    /// <param name="pooledObject"></param>
    /// <param name="parent"></param>
    /// <param name="numToSpawn"></param>
    public ObjectPool(GameObject pooledObject, Transform parent, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        Spawn(numToSpawn, parent);
    }

    /// <summary>
    /// Constructor for the object pool.
    /// </summary>
    /// <param name="pooledObject"></param>
    /// <param name="pullObject"></param>
    /// <param name="pushObject"></param>
    /// <param name="numToSpawn"></param>
    public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        this.pullObject = pullObject;
        this.pushObject = pushObject;
        Spawn(numToSpawn);
    }

    /// <summary>
    /// Constructor for the object pool.
    /// </summary>
    /// <param name="pooledObject"></param>
    /// <param name="pullObject"></param>
    /// <param name="pushObject"></param>
    /// <param name="parent"></param>
    /// <param name="numToSpawn"></param>
    public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, Transform parent, int numToSpawn = 0)
    {
        this.prefab = pooledObject;
        this.pullObject = pullObject;
        this.pushObject = pushObject;
        Spawn(numToSpawn, parent);
    }

    /// <summary>
    /// Gets the current number of objects in the pool.
    /// </summary>
    public int pooledCount
    {
        get
        {
            return pooledObjects.Count;
        }
    }

    /// <summary>
    /// This method pulls an object from the pool.
    /// </summary>
    /// <returns></returns>
    public T Pull()
    {
        T t;
        if (pooledCount > 0)
            t = pooledObjects.Pop();
        else
            t = GameObject.Instantiate(prefab).GetComponent<T>();

        t.gameObject.SetActive(true); //ensure the object is on
        t.Initialize(Push);

        //allow default behavior and turning object back on
        pullObject?.Invoke(t);

        return t;
    }

    /// <summary>
    /// This method pulls an object from the pool and sets its position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public T Pull(Vector3 position)
    {
        T t = Pull();
        t.transform.position = position;
        return t;
    }

    /// <summary>
    /// This method pulls an object from the pool and sets its parent.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Pull(Transform parent)
    {
        T t = Pull();
        t.transform.SetParent(parent);
        return t;
    }

    /// <summary>
    /// This method pulls an object from the pool and sets its position and rotation.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public T Pull(Vector3 position, Quaternion rotation)
    {
        T t = Pull();
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    /// <summary>
    /// This method pulls an object from the pool and sets its position and parent.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Pull(Vector3 position, Transform parent)
    {
        T t = Pull();
        t.transform.position = position;
        t.transform.SetParent(parent);
        return t;
    }

    /// <summary>
    /// This method pushes an object back into the pool.
    /// </summary>
    /// <param name="t"></param>
    public void Push(T t)
    {
        pooledObjects.Push(t);

        //create default behavior to turn off objects
        pushObject?.Invoke(t);

        t.gameObject.SetActive(false);
    }

    /// <summary>
    /// This method spawns a number of objects into the pool.
    /// </summary>
    /// <param name="number"></param>
    private void Spawn(int number)
    {
        T t;

        for (int i = 0; i < number; i++)
        {
            t = GameObject.Instantiate(prefab).GetComponent<T>();
            pooledObjects.Push(t);
            t.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This method spawns a number of objects into the pool and sets their parent.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="parent"></param>
    private void Spawn(int number, Transform parent)
    {
        T t;

        for (int i = 0; i < number; i++)
        {
            t = GameObject.Instantiate(prefab, parent).GetComponent<T>();
            pooledObjects.Push(t);
            t.gameObject.SetActive(false);
        }
    }
}
