using System;
using UnityEngine;

public class VFXController : MonoBehaviour, IPoolable<VFXController>
{
    private Action<VFXController> _returnAction;
    public void Initialize(Action<VFXController> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }
}
