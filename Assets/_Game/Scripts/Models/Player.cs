using UnityEngine;

public class Player : CharacterBase
{
    #region Fields
    [Header("Target Aim Components"), Space(5f)]
    [SerializeField] private Transform _targetLock;
    [SerializeField] private float _targetLockSpeed;
    [SerializeField] private float _groundOffset;

    // Cached variables.
    private float _horizontal;
    private float _vertical;
    #endregion

    #region Methods
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected override void Start()
    {
        weaponData = WeaponManager.Instance.GetWeaponDataByIndex(GameplayManager.Instance.UserData.EquippedWeapon);
        EquipWeapon(weaponData);
        pantSkin.material = GameplayManager.Instance.GetPantByIndex(GameplayManager.Instance.UserData.EquippedPant);
        base.Start();
    }

    /// <summary>
    /// FixedUpdate is called once per frame.
    /// </summary>
    protected override void FixedUpdate()
    {
        switch (GameplayManager.Instance.GameState)
        {
            case GameState.Preparing:
                isIdle = true;
                break;
            case GameState.Playing:
                PlayerInput();
                base.FixedUpdate();
                break;
            case GameState.Paused:
                animator.speed = 0f;
                break;
            case GameState.GameOver:

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void LateUpdate()
    {
        switch (GameplayManager.Instance.GameState)
        {
            case GameState.Preparing:
                break;
            case GameState.Playing:
                base.LateUpdate();
                LockTarget();
                break;
            case GameState.Paused:

                break;
            case GameState.GameOver:

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Listen to player input.
    /// </summary>
    private void PlayerInput()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        direction = new Vector3(_horizontal, 0f, _vertical).normalized;
        bool isMoving = direction != Vector3.zero;

        if (!isMoving && !isIdle)
        {
            isIdle = true;
            SetAnimationParameters();
        }
        else
        {
            isIdle = !isMoving;
        }


        if (Input.GetMouseButtonDown(0))
        {
            WeaponManager.Instance.GetWeapon(weaponData.WeaponType, weaponData.ThrowWeaponPrefab)
            .Throw(weaponHolder.position, direction, attackRange, scaleValue, this, weaponData, OnGetKill);
        }
    }

    /// <summary>
    /// Locks the target.
    /// </summary>
    private void LockTarget()
    {
        if (target is null)
        {
            _targetLock.gameObject.SetActive(false);
            _targetLock.position = Vector3.Lerp(_targetLock.position, m_transform.position, _targetLockSpeed);
            _targetLock.position += Vector3.up * _groundOffset;
        }
        else
        {
            _targetLock.gameObject.SetActive(true);
            _targetLock.position = Vector3.Lerp(_targetLock.position, target.transform.position, _targetLockSpeed);
            _targetLock.position += Vector3.up * _groundOffset;
        }
    }

    /// <summary>
    /// OnGetKill is called when the weapon that the character throws hits another character.
    /// </summary>
    /// <param name="target"></param>
    protected override void OnGetKill(CharacterBase target)
    {
        base.OnGetKill(target);
        if (isDead) return;
        CameraManager.Instance.ZoomOutGamePlayCamera(scaleValue);
    }

    /// <summary>
    /// OnDead is called when the character dies.
    /// </summary>
    public override void OnDead()
    {
        base.OnDead();
        UIManager.Instance.OpenReviveUI();
    }

    public void Revive()
    {
        isDead = false;
        radarController.gameObject.SetActive(true);
        SetAnimationParameters();
        BotManager.Instance.ForceSpawnAll();
    }
    #endregion
}
