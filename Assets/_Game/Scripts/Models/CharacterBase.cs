using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _weaponHolder;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Collider _hitCollider;
    [SerializeField] protected WeaponBase _weapon;
    [SerializeField] protected Canvas _infoCanvas;
    [SerializeField] protected SkinnedMeshRenderer _pantSkin;
    [SerializeField] protected RadarController _radarController;

    [Header("Character Stats"), Space(5f)]
    [SerializeField] protected float _rotateSpeed;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _maxLocalScale;
    [SerializeField] protected float _localScaleIncreaseValue;

    protected const string IDLE_ANIMATION = "IsIdle";
    protected const string WIN_ANIMATION = "IsWin";
    protected const string ATTACK_ANIMATION = "IsAttack";
    protected const string DEAD_ANIMATION = "IsDead";
    protected const string DANCE_ANIMATION = "IsDance";
    protected const string ULTI_ANIMATION = "IsUlti";

    protected bool _isIdle = true;
    protected bool _isDead = false;
    protected bool _isWin = false;
    protected bool _isAttack = false;
    protected bool _isDance = false;
    protected bool _isUlti = false;
    protected bool _inAttackProcess = false;

    protected int _killCount = 0;
    protected Vector3 _direction = Vector3.zero;
    protected CharacterBase _target;
    protected List<CharacterBase> _targetsList = new List<CharacterBase>();

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        _pantSkin.material = GameplayManager.Instance.GetPantByIndex();
        _radarController?.OnEnemyEnterCallBack(OnFoundTarget);
        _radarController?.OnEnemyExitCallBack(OnLostTarget);
    }

    /// <summary>
    /// FixedUpdate is called once per frame.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        Movement();
        Attack();
        SetAnimationParameters();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        CanvasController();
    }

    /// <summary>
    /// Controls the movement of the character.
    /// </summary>
    private void Movement()
    {
        if (_isAttack || _isIdle || _isWin || _isDead) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotateSpeed);
        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, _moveSpeed * Time.fixedDeltaTime);
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
        weapon.transform.position = _weaponHolder.position;
        weapon.SetDestination(transform.position, transform.forward, OnGetKill, this);
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
    /// Called when the weapon that the character throws hits another character.
    /// </summary>
    protected void OnGetKill()
    {
        _killCount++;
        transform.localScale = (_maxLocalScale - _localScaleIncreaseValue / (_localScaleIncreaseValue + _killCount)) * Vector3.one;
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Weapon)
        {
            WeaponBase weapon = other.GetComponent<WeaponBase>();
            if (weapon.Caster != this)
            {
                weapon.OnHit();
                _isDead = true;
            }
        }
    }

    protected void OnFoundTarget(CharacterBase target)
    {
        if (_target is null)
        {
            _target = target;
        }
        else
        {
            _targetsList.Add(target);
        }
    }

    protected void OnLostTarget(CharacterBase target)
    {
        if (_target == target)
        {
            _target = null;
            if (_targetsList.Count >= 1)
            {
                _target = _targetsList?[0];
                _targetsList?.RemoveAt(0);
            }
        }
        else
        {
            _targetsList.Remove(target);
        }
    }
}
