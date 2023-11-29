using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IPoolable<BulletBase>
{
    protected Action<BulletBase> _returnAction;
    public void Initialize(Action<BulletBase> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }

    public virtual void Throw(Vector3 spawnPosition, Vector3 direction, float characterRange, float scaleValue, CharacterBase attacker, WeaponData weaponData, Action<CharacterBase> callback)
    {

    }
}
