using Cinemachine;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _weaponHolder;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected WeaponBase _weapon;
    [SerializeField] protected Canvas _infoCanvas;

    [Header("Character Stats"), Space(5f)]
    [SerializeField] protected float _rotateSpeed;
    [SerializeField] protected float _moveSpeed;

    protected static string IDLE_ANIMATION = "IsIdle";
    protected static string WIN_ANIMATION = "IsWin";
    protected static string ATTACK_ANIMATION = "IsAttack";
    protected static string DEAD_ANIMATION = "IsDead";
    protected static string DANCE_ANIMATION = "IsDance";
    protected static string ULTI_ANIMATION = "IsUlti";

    protected bool _isIdle = true;
    protected bool _isDead = false;
    protected bool _isWin = false;
    protected bool _isAttack = false;
    protected bool _isDance = false;
    protected bool _isUlti = false;
    protected bool _inAttackProcess = false;

    protected virtual void Start()
    {

    }
    protected virtual void FixedUpdate()
    {
        Attack();
        SetAnimationParameters();
    }

    protected virtual void Update()
    {
        CanvasController();
    }

    /// <summary>
    /// Controls the attack of the character.
    /// </summary>
    private void Attack()
    {
        if (!_isAttack || _inAttackProcess || _isDead || _isWin || _isUlti) return;
        _inAttackProcess = true;
        Invoke(nameof(ThrowWeapon), .15f);
        Invoke(nameof(EndAttackProcess), .65f);
    }

    /// <summary>
    /// Spawns the weapon and throws it.
    /// </summary>
    private void ThrowWeapon()
    {
        var weapon = Instantiate(_weapon);
        Physics.IgnoreCollision(weapon.Collider, this._collider);
        weapon.transform.position = _weaponHolder.position;
        weapon.SetDestination(transform.position, transform.forward);
    }

    /// <summary>
    /// The end of the attack process.
    /// </summary>
    private void EndAttackProcess()
    {
        _inAttackProcess = false;
        _isAttack = false;
    }

    /// <summary>
    /// Sets the animation parameters.
    /// </summary>
    private void SetAnimationParameters()
    {
        _animator.SetBool(DEAD_ANIMATION, _isDead);
        _animator.SetBool(WIN_ANIMATION, _isWin);
        _animator.SetBool(DANCE_ANIMATION, _isDance);
        _animator.SetBool(ULTI_ANIMATION, _isUlti);
        _animator.SetBool(ATTACK_ANIMATION, _isAttack);
        _animator.SetBool(IDLE_ANIMATION, _isIdle);
    }

    /// <summary>
    /// Make the canvas look at the camera.
    /// </summary>
    private void CanvasController()
    {
        _infoCanvas.transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Weapon)
        {
            Debug.Log("Player is dead");
            _isDead = true;
        }
    }
}
