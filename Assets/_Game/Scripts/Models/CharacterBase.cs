using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _weaponHolder;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Collider _hitCollider;
    [SerializeField] protected RadarController _radarController;
    [SerializeField] protected WeaponBase _weapon;
    [SerializeField] protected Canvas _infoCanvas;
    [SerializeField] protected SkinnedMeshRenderer _pantSkin;

    [Header("Character Stats"), Space(5f)]
    [SerializeField] protected float _rotateSpeed;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _maxLocalScale;
    [SerializeField] protected float _localScaleIncreaseValue;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _attackSpeed;

    // Animation name constants.
    protected const string IDLE_ANIMATION = "IsIdle";
    protected const string WIN_ANIMATION = "IsWin";
    protected const string ATTACK_ANIMATION = "IsAttack";
    protected const string DEAD_ANIMATION = "IsDead";
    protected const string DANCE_ANIMATION = "IsDance";
    protected const string ULTI_ANIMATION = "IsUlti";

    // Animation parameters.
    protected bool _isIdle = true;
    protected bool _isDead = false;
    protected bool _isWin = false;
    protected bool _isAttack = false;
    protected bool _isDance = false;
    protected bool _isUlti = false;

    // Current weapon data.
    protected WeaponData _weaponData;

    // Targets in range.
    protected CharacterBase _target;
    protected List<CharacterBase> _targetsList = new List<CharacterBase>();

    protected int _killCount = 0;
    protected float _attackTimer = 0f;
    protected Vector3 _direction = Vector3.zero;

    protected virtual void OnEnable()
    {

    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {


        Physics.IgnoreLayerCollision((int)LayerType.Weapon, (int)LayerType.Radar, true);

        _radarController.OnEnemyEnterCallBack(OnFoundTarget);
        _radarController.OnEnemyExitCallBack(OnLostTarget);
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

    public void EquipWeapon(WeaponData weaponData)
    {
        if (_weaponHolder.childCount > 0)
        {
            Destroy(_weaponHolder.GetChild(0).gameObject);
        }
        _weaponData = weaponData;
        var weapon = Instantiate(_weaponData.WeaponModel, _weaponHolder);
        weapon.transform.localScale = _weaponData.HandWeaponScale * Vector3.one;
        weapon.transform.localPosition = _weaponData.HandWeaponOffset;
        weapon.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Controls the movement of the character.
    /// </summary>
    private void Movement()
    {
        if (_isAttack || _isIdle || _isWin || _isDead) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(_direction),
            _rotateSpeed);


        transform.position = Vector3.Lerp(
            transform.position,
            transform.position + _direction,
            _moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Controls the attack of the character.
    /// </summary>
    private void Attack()
    {
        // Cooldown
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.fixedDeltaTime;
        }

        // Requirements conditions
        if (!_target || !_isIdle || _isDead || _isWin || _isUlti) return;

        // Look at the target
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(_target.transform.position - transform.position),
            _rotateSpeed);

        // Can attack
        if (!_isAttack && _attackTimer <= 0)
        {
            _isAttack = true;
            if (_weaponData.WeaponType == WeaponType.Boomerang) _isUlti = true;
            _attackTimer += _weaponData.BonusAttackSpeed;
            Invoke(nameof(ThrowWeapon), _weaponData.ThrowAnimationDelay);
            Invoke(nameof(EndAttackProcess), _weaponData.ThrowAnimationTotalLength);
        }
    }

    /// <summary>
    /// Spawns the weapon and throws it.
    /// </summary>
    private void ThrowWeapon()
    {
        Vector3 direction = (_target.transform.position - this._weaponHolder.position).normalized;
        direction.y = 0f;
        _weaponData.Throw(_weaponHolder.position, direction, _attackRange, this, OnGetKill, _hitCollider);
    }

    /// <summary>
    /// The end of the attack process.
    /// </summary>
    private void EndAttackProcess()
    {
        _isUlti = false;
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
    protected void OnGetKill(CharacterBase target)
    {
        if (_isDead) return;
        if (_target == target) _target = null;
        else _targetsList.Remove(target);

        _killCount++;
        transform.localScale = (_maxLocalScale - _localScaleIncreaseValue / (_localScaleIncreaseValue + _killCount)) * Vector3.one;
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;
        if (other.gameObject.layer == (byte)LayerType.Weapon)
        {
            WeaponBase weapon = other.GetComponent<WeaponBase>();
            if (weapon.Attacker != this)
            {
                weapon.OnHit(this);
                _isDead = true;
                Destroy(gameObject, 2f);
            }
        }
    }

    /// <summary>
    /// OnFoundTarget is called when the radar detects an enemy.
    /// </summary>
    /// <param name="target"></param>
    protected void OnFoundTarget(CharacterBase target)
    {
        if (target == this) return;
        if (_target is null)
        {
            _target = target;
        }
        else
        {
            _targetsList.Add(target);
        }
    }

    /// <summary>
    /// OnLostTarget is called when the radar loses an enemy.
    /// </summary>
    /// <param name="target"></param>
    protected void OnLostTarget(CharacterBase target)
    {
        if (_target == target)
        {
            _target = null;
            if (_targetsList.Count > 0)
            {
                _target = _targetsList[0];
                _targetsList.RemoveAt(0);
            }
        }
        else
        {
            _targetsList.Remove(target);
        }
    }
}
