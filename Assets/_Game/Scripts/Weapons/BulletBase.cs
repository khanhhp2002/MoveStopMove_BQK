using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public virtual void Throw(Vector3 spawnPosition, Vector3 direction, float characterRange, float scaleValue, CharacterBase attacker, WeaponData weaponData, Action<CharacterBase> callback)
    {

    }
}
