using UnityEngine;

public class Player : CharacterBase
{
    #region Fields
    [Header("Target Aim Components"), Space(5f)]
    [SerializeField] private Transform _targetLock;
    [SerializeField] private float _targetLockSpeed;
    [SerializeField] private float _groundOffset;
    [SerializeField] private GameObject _attackRangeVisual;

    /// <summary>
    /// Change IsDance state.
    /// </summary>
    public bool IsDance
    {
        get => isDance;
        set
        {
            isDance = value;
            SetAnimationParameters();
        }
    }

    // Cached variables.
    private int _ranking;
    private string _killerName;

    public string KillerName
    {
        get => _killerName;
        set => _killerName = value;
    }

    public int Ranking { get => _ranking; set => _ranking = value; }

    #endregion

    #region Methods
    private void Awake()
    {
        OnCharacterChangeWeapon += OnPlayerChangeWeapon;
    }
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected override void Start()
    {
        EquipWeapon(WeaponManager.Instance.GetWeaponDataByIndex(GameplayManager.Instance.UserData.EquippedWeapon));
        EquipPant(RuntimeData.Instance.SkinStorage.EquipPant(GameplayManager.Instance.UserData.EquippedPant));
        EquipHair(RuntimeData.Instance.SkinStorage.Hairs[GameplayManager.Instance.UserData.EquippedHair].Model);
        characterName.text = GameplayManager.Instance.UserData.Name;
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
        //_horizontal = Input.GetAxis("Horizontal");
        //_vertical = Input.GetAxis("Vertical");
        //direction = new Vector3(_horizontal, 0f, _vertical).normalized;
        direction = JoyStick.Instance.GetDirection();
        direction = new Vector3(direction.x, 0f, direction.y).normalized;
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
    }

    private void OnPlayerChangeWeapon()
    {
        _attackRangeVisual.transform.localScale = Vector3.one * (attackRange + weaponData.BonusAttackRange) * 2;
        GameplayUI.Instance.OnChangeWeapon(attackSpeed - weaponData.BonusAttackSpeed);
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
        if (GameplayManager.Instance.UserData.IsVibrationEnabled)
            Handheld.Vibrate();
        SoundManager.Instance.PlaySFX(SFXType.Death);
        if (GameplayManager.Instance.AliveCounter is not 1)
            UIManager.Instance.OpenReviveUI();
    }

    /// <summary>
    /// Revives the player.
    /// </summary>
    public void Revive()
    {
        isDead = false;
        radarController.gameObject.SetActive(true);
        SetAnimationParameters();
        //BotManager.Instance.ForceSpawnAll();
    }
    #endregion
}
