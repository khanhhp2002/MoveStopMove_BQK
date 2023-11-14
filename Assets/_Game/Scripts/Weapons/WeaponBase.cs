using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Components"), Space(5f)]
    [SerializeField] private Collider _collider;

    private Action<CharacterBase> _onGetHit1;
    private Action<WeaponBase> _onGetHit2;
    private CharacterBase _attacker;

    /// <summary>
    /// Initialize weapon.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="callbackAttacker"></param>
    /// <param name="callbackWeapon"></param>
    public void Init(CharacterBase attacker, Action<CharacterBase> callbackAttacker, Action<WeaponBase> callbackWeapon, Collider attackerCollider)
    {
        Physics.IgnoreCollision(_collider, attackerCollider, true);
        _attacker = attacker;
        _onGetHit1 = callbackAttacker;
        _onGetHit2 = callbackWeapon;
    }

    /// <summary>
    /// Get the attacker of the weapon.
    /// </summary>
    public CharacterBase Attacker => _attacker;

    /// <summary>
    /// Get the collider of the weapon.
    /// </summary>
    public Collider Collider => _collider;

    /// <summary>
    /// Called when the weapon hit character.
    /// </summary>
    public void OnHit(CharacterBase target)
    {
        if (_attacker == target) return;
        _onGetHit1?.Invoke(target);
        _onGetHit2?.Invoke(this);
    }

}
