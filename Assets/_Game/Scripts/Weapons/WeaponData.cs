using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WeaponData
{
    [Header("Weapon Info"), Space(5f)]
    public string Name;
    public WeaponType WeaponType;

    [Header("Weapon 3D"), Space(5f)]
    public ThrowWeapon ThrowWeaponPrefab;
    public GameObject WeaponModel;

    [Header("Hand-Weapon Settings"), Space(5f)]
    public Vector3 HandWeaponOffset;
    public float HandWeaponScale;

    [Header("Throw-Weapon Settings"), Space(5f)]
    public Ease MoveType;
    [Range(1f, 10f)] public float MoveSpeed;
    [Range(0, 540)] public int RotateSpeed;
    public float BonusAttackRange;
    public float BonusAttackSpeed;

    [Header("Throw-weapon abilities"), Space(5f)]
    public bool IsPiercingable;
    public bool IsReturningable;

    [Header("Purchase-weapon settings"), Space(5f)]
    public float ScreenWeaponOffsetX;
    [Range(-180f, 180f)] public float ScreenWeaponZAngle;
    public float ScreenWeaponScale;
    [Min(1)] public int PurchasePrice;
    public string PurchaseRequirement;
}
