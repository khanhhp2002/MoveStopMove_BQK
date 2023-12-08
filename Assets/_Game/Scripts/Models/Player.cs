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
    private string _killerName;

    public string KillerName
    {
        get => _killerName;
        set => _killerName = value;
    }
    #endregion

    #region Methods
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
            if (m_hair is not null)
            {
                Destroy(m_hair.gameObject);
            }
            m_hair = Instantiate(RuntimeData.Instance.SkinStorage.Hairs[UnityEngine.Random.Range(0, RuntimeData.Instance.SkinStorage.Hairs.Count)].Model, hairContainer);
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
        if (GameplayManager.Instance.UserData.IsVibrationEnabled)
            Handheld.Vibrate();
        SoundManager.Instance.PlaySFX(SFXType.Death);
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
        BotManager.Instance.ForceSpawnAll();
    }
    #endregion
}
