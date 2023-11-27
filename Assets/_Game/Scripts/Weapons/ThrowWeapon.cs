using DG.Tweening;
using System;
using UnityEngine;

public class ThrowWeapon : BulletBase
{
    [Header("Weapon Components"), Space(5f)]
    [SerializeField] private Collider _collider;

    private Action<CharacterBase> _onGetHit;
    private CharacterBase _attacker;
    private bool _isPiercingable;
    public Vector3 MoveDirection { get; private set; }

    /// <summary>
    /// Throw weapon.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="direction"></param>
    /// <param name="characterRange"></param>
    /// <param name="scaleValue"></param>
    /// <param name="attacker"></param>
    /// <param name="callback"></param>
    /// <param name="weaponData"></param>
    public override void Throw(Vector3 spawnPosition, Vector3 direction, float characterRange, float scaleValue, CharacterBase attacker, WeaponData weaponData, Action<CharacterBase> callBack)
    {
        // Setup data
        Vector3 destination = spawnPosition + direction * (characterRange * scaleValue + weaponData.BonusAttackRange);

        float duration = Vector3.Distance(spawnPosition, destination) / weaponData.MoveSpeed;

        transform.position = spawnPosition;

        transform.localScale *= scaleValue;

        // Move weapon
        transform.DOMove(destination, duration)
            .SetEase(weaponData.MoveType)
            .SetLoops(weaponData.IsReturningable ? 2 : 0, LoopType.Yoyo) // LoopType.Yoyo is fixed value
            .OnComplete(() => DestroyWeapon());

        // Rotate weapon
        if (weaponData.RotateSpeed > 0)
        {
            Vector3 rotateDelta = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 180f, transform.eulerAngles.z);

            float rotateDeltaDuration = 1f / (weaponData.RotateSpeed / 180f);

            transform.DORotate(rotateDelta, rotateDeltaDuration)
                .SetLoops(-1, LoopType.Incremental) // LoopType.Incremental is fixed value
                .SetEase(Ease.Linear); // Ease.Linear is fixed value
        }

        // Initialize weapon
        _isPiercingable = weaponData.IsPiercingable;
        _attacker = attacker;
        _onGetHit = callBack;
        MoveDirection = direction;
        _collider.enabled = true;
    }

    /// <summary>
    /// Destroy weapon.
    /// </summary>
    /// <param name="weapon"></param>
    private void DestroyWeapon()
    {
        DOTween.Kill(transform);
        Destroy(gameObject);
    }

    /// <summary>
    /// Get the attacker of the weapon.
    /// </summary>
    public CharacterBase Attacker => _attacker;

    /// <summary>
    /// Called when this weapon hit character.
    /// </summary>
    private void OnHit(CharacterBase target)
    {
        if (!_isPiercingable)
        {
            DestroyWeapon();
        }
        if (_attacker == target) return;
        _onGetHit?.Invoke(target);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // When the weapon hits the character.
        if (other.gameObject.layer == (byte)LayerType.Character)
        {
            CharacterBase character = other.GetComponent<CharacterBase>();
            if (_attacker != character)
            {
                OnHit(character);
                character.OnDead();
            }
        }
    }
}
