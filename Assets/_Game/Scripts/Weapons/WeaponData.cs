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
    public WeaponBase WeaponPrefab;
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
    public float ThrowAnimationDelay;
    public float ThrowAnimationTotalLength;

    [Header("Throw-weapon abilities"), Space(5f)]
    public bool IsPiercingable;
    public bool IsReturningable;

    [Header("Purchase-weapon settings"), Space(5f)]
    public float ScreenWeaponOffsetX;
    [Range(-180f, 180f)] public float ScreenWeaponZAngle;
    public float ScreenWeaponScale;
    [Min(1)] public int PurchasePrice;
    public string PurchaseRequirement;

    /// <summary>
    /// Throw weapon logic.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="startPosition"></param>
    /// <param name="direction"></param>
    /// <param name="characterRange"></param>
    public void Throw(Vector3 spawnPosition, Vector3 direction, float characterRange, CharacterBase _attacker, Action<CharacterBase> callback, Collider attackerCollider)
    {
        Vector3 destination = spawnPosition + direction * (characterRange + BonusAttackRange);

        float duration = Vector3.Distance(spawnPosition, destination) / MoveSpeed;

        WeaponBase weapon = GameObject.Instantiate(WeaponPrefab);

        weapon.transform.position = spawnPosition;

        // Move weapon
        weapon.transform.DOMove(destination, duration)
            .SetEase(MoveType)
            .SetLoops(IsReturningable ? 2 : 0, LoopType.Yoyo) // LoopType.Yoyo is fixed value
            .OnComplete(() => DestroyWeapon(weapon));

        // Rotate weapon
        if (RotateSpeed > 0)
        {
            Vector3 rotateDelta = new Vector3(weapon.transform.eulerAngles.x, weapon.transform.eulerAngles.y - 180f, weapon.transform.eulerAngles.z);

            float rotateDeltaDuration = 1f / (RotateSpeed / 180f);

            weapon.transform.DORotate(rotateDelta, rotateDeltaDuration)
                .SetLoops(-1, LoopType.Incremental) // LoopType.Incremental is fixed value
                .SetEase(Ease.Linear); // Ease.Linear is fixed value
        }

        // Initialize weapon
        weapon.Init(_attacker, callback, OnHit, attackerCollider);

        weapon.Collider.enabled = true;
    }

    /// <summary>
    /// OnHit call when the weapon hits the target.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="weapon"></param>
    public void OnHit(WeaponBase weapon)
    {
        if (!IsPiercingable)
        {
            DestroyWeapon(weapon);
        }
    }

    /// <summary>
    /// Destroy weapon.
    /// </summary>
    /// <param name="weapon"></param>
    private void DestroyWeapon(WeaponBase weapon)
    {
        DOTween.Kill(weapon.transform);
        GameObject.Destroy(weapon.gameObject);
    }
}
