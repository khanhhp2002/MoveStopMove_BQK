using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _weaponHolder;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected WeaponBase _weapon;

    [Header("Character Stats"), Space(5f)]
    [SerializeField] protected float _rotateSpeed;
    [SerializeField] protected float _moveSpeed;

    protected static string IDLE_ANIMATION = "IsIdle";
    protected static string WIN_ANIMATION = "IsWin";
    protected static string ATTACK_ANIMATION = "IsAttack";
    protected static string DEAD_ANIMATION = "IsDead";
    protected static string DANCE_ANIMATION = "IsDead";
    protected static string ULTI_ANIMATION = "IsUlti";

    protected bool _isIdle = true;
    protected bool _isDead = false;
    protected bool _isWin = false;
    protected bool _isAttack = false;
    protected bool _isDance = false;
    protected bool _isUlti = false;
    protected bool _inAttackProcess = false;
}
