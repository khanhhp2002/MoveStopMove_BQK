using System;

/// <summary>
/// Rules for the object pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPoolable<T>
{
    void Initialize(Action<T> returnAction);
    void ReturnToPool();
}
