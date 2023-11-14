using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public string Name;
    public WeaponType WeaponType;
    public WeaponBase WeaponPrefab;
    public Ease MoveType;
    [Range(1f, 10f)] public float MoveSpeed;
    [Range(0f, 540f)] public float RotateSpeed;
    public float AttackRange;
    public float AttackSpeed;
    public float ThrowAnimationDelay;
    public float ThrowAnimationTotalLength;
    public bool IsPiercingable;
    public bool IsReturningable;

    /// <summary>
    /// Throw weapon logic.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="startPosition"></param>
    /// <param name="direction"></param>
    /// <param name="characterRange"></param>
    public void Throw(Vector3 spawnPosition, Vector3 startPosition, Vector3 direction, float characterRange)
    {
        Vector3 destination = startPosition + direction * (characterRange + AttackRange);

        float duration = Vector3.Distance(startPosition, destination) / MoveSpeed;

        WeaponBase weapon = GameObject.Instantiate(WeaponPrefab);

        weapon.transform.position = spawnPosition;

        weapon.transform.DOMove(destination, duration)
            .SetEase(MoveType)
            .SetLoops(IsReturningable ? 2 : 0, LoopType.Yoyo)
            .OnComplete(() =>
            {
                DOTween.Kill(weapon.transform);
                GameObject.Destroy(weapon.gameObject);
            });

        if (RotateSpeed > 0)
        {
            Vector3 rotateDelta = new Vector3(weapon.transform.eulerAngles.x, weapon.transform.eulerAngles.y + 180f, weapon.transform.eulerAngles.z);

            float rotateDeltaDuration = 1f / (RotateSpeed / 180f);

            weapon.transform.DORotate(rotateDelta, rotateDeltaDuration)
                .SetLoops(-1, LoopType.Incremental) // LoopType.Incremental is fixed value
                .SetEase(Ease.Linear); // Ease.Linear is fixed value
        }
    }
}
