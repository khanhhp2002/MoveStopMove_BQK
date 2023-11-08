using System;
using UnityEngine;

public class NavigationIndicator : MonoBehaviour, IPoolable<NavigationIndicator>
{
    private Action<NavigationIndicator> _returnAction;

    /// <summary>
    /// Initializes the navigation indicator.
    /// </summary>
    /// <param name="returnAction"></param>
    public void Initialize(Action<NavigationIndicator> returnAction)
    {
        _returnAction = returnAction;
    }

    /// <summary>
    /// Returns the navigation indicator to the pool.
    /// </summary>
    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }
}
