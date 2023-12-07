using System;
using UnityEngine;

public class Bot : CharacterBase, IPoolable<Bot>
{
    #region Fields
    [Header("Bot Components"), Space(5f)]
    [SerializeField, Range(0.1f, 0.5f)] private float _navigationIndicatorSpeed;
    [SerializeField] private float _screenMarginValue;

    [Header("Bot Stats"), Space(5f)]
    [SerializeField] private BotState _botState;
    [SerializeField, Range(0f, 100f)] private byte _botDogdeChance;

    // Cached variables.
    private IState _currentState;
    private Vector2 _botPos;
    private Vector2 _playerPos;
    private Vector2 _directionToPlayer;
    private Vector2 _indicatorPos;
    private bool _isIgnoreAttack = false;
    private NavigationIndicator _navigationIndicator;
    private Vector4 _screenMargin; // Left, Right, Top, Bottom
    private bool _isOnScreen = true;

    // Event.
    private Action<Bot> _returnPoolAction;
    #endregion

    #region Properties
    public bool IsIgnoreAttack
    {
        get => _isIgnoreAttack;
        set
        {
            _isIgnoreAttack = value;
            if (value)
            {
                Invoke(nameof(ResetIgnoreAttack), UnityEngine.Random.Range(2f, 4f));
            }
        }
    }
    public CharacterBase Target => target;
    #endregion

    #region Methods
    protected override void OnEnable()
    {
        weaponData = WeaponManager.Instance.GetRandomWeaponData();
        EquipWeapon(weaponData);
        pantSkin.material = GameplayManager.Instance.SkinSO.EquipPant();
        _currentState = new IdleState();
        characterName.text = RandomStringGenerator.GetRandomString(UnityEngine.Random.Range(5, 10));
        base.OnEnable();
    }

    protected override void Start()
    {
        _screenMargin = new Vector4(
                       _screenMarginValue,
                        Screen.width - _screenMarginValue,
                        Screen.height - _screenMarginValue,
                        _screenMarginValue);
        radarController.OnWallDetectedCallBack(OnWallDetected);
        radarController.OnBulletDetectedCallBack(OnBulletDetected);
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

                break;
            case GameState.Playing:

                _currentState?.OnExecute(this);
                base.FixedUpdate();
                break;
            case GameState.Paused:

                break;
            case GameState.GameOver:
                ReturnToPool();
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
                NavigationIndicatorControl();
                if (_isOnScreen)
                {
                    base.LateUpdate();
                }
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
    /// Controls the indicator that shows the bot's position when it is out of the screen.
    /// </summary>
    private void NavigationIndicatorControl()
    {
        if (GameplayManager.Instance.Player is null) return;

        _botPos = Camera.main.WorldToScreenPoint(m_transform.position);

        if (Vector3.Dot(m_transform.position - Camera.main.transform.position, Camera.main.transform.forward) < 0) // Behind the camera?
        {
            _botPos.x = Screen.width - _botPos.x;
            _botPos.y = Screen.height - _botPos.y;
        }

        if (_botPos.x < -10f || _botPos.x > Screen.width + 10f || _botPos.y < -10f || _botPos.y > Screen.height + 10f) // Out of screen
        {
            _isOnScreen = false;
            _playerPos = Camera.main.WorldToScreenPoint(GameplayManager.Instance.Player.transform.position);
            _directionToPlayer = (_botPos - _playerPos).normalized;
            _indicatorPos = FindIndicatorPosition();

            if (_navigationIndicator is null)
            {
                _navigationIndicator = NavigationIndicatorManager.Instance.Spawn(_indicatorPos);
                _navigationIndicator.SetPoint(point);
            }
            else
            {
                _navigationIndicator.transform.position = Vector2.Lerp(
                    _navigationIndicator.transform.position,
                    _indicatorPos,
                    _navigationIndicatorSpeed);

                _navigationIndicator.LookAt(_directionToPlayer);
            }
        }
        else // In screen
        {
            _isOnScreen = true;
            if (_navigationIndicator is not null)
            {
                _navigationIndicator.ReturnToPool();
                _navigationIndicator = null;
            }
        }
    }

    /// <summary>
    /// Finds the position of the indicator.
    /// </summary>
    /// <returns></returns>
    private Vector2 FindIndicatorPosition()
    {
        // y = m_slope * x + m_intercept
        float m_slope = _directionToPlayer.y / _directionToPlayer.x;
        float m_intercept = _botPos.y - m_slope * _botPos.x;

        // calculate points
        Vector2 leftPoint = new Vector2(_screenMargin.x, m_slope * _screenMargin.x + m_intercept);
        Vector2 rightPoint = new Vector2(_screenMargin.y, m_slope * _screenMargin.y + m_intercept);
        Vector2 topPoint = new Vector2((_screenMargin.z - m_intercept) / m_slope, _screenMargin.z);
        Vector2 bottomPoint = new Vector2((_screenMargin.w - m_intercept) / m_slope, _screenMargin.w);

        // find valid point
        if (_botPos.x < _playerPos.x && leftPoint.x >= _screenMargin.x && leftPoint.x <= _screenMargin.y && leftPoint.y >= _screenMargin.w && leftPoint.y <= _screenMargin.z)
        {
            return leftPoint;
        }
        else if (_botPos.x > _playerPos.x && rightPoint.x >= _screenMargin.x && rightPoint.x <= _screenMargin.y && rightPoint.y >= _screenMargin.w && rightPoint.y <= _screenMargin.z)
        {
            return rightPoint;
        }
        else if (_botPos.y > _playerPos.y && topPoint.x >= _screenMargin.x && topPoint.x <= _screenMargin.y && topPoint.y >= _screenMargin.w && topPoint.y <= _screenMargin.z)
        {
            return topPoint;
        }
        else
        {
            return bottomPoint;
        }
    }

    /// <summary>
    /// Resets the ignore attack state.
    /// </summary>
    private void ResetIgnoreAttack()
    {
        _isIgnoreAttack = false;
    }

    /// <summary>
    /// Sets the point of the navigation indicator.
    /// </summary>
    private void SetPoint()
    {
        if (_navigationIndicator is not null)
            _navigationIndicator.SetPoint(point);
    }

    /// <summary>
    /// Initialize the bot return action.
    /// </summary>
    /// <param name="returnAction"></param>
    public void Initialize(Action<Bot> returnAction)
    {
        _returnPoolAction = returnAction;
    }

    /// <summary>
    /// Returns the bot to the pool.
    /// </summary>
    public void ReturnToPool()
    {
        _returnPoolAction?.Invoke(this);
    }

    /// <summary>
    /// Sets the state of the bot.
    /// </summary>
    /// <param name="state"></param>
    public void SetState(IState state)
    {
        _currentState?.OnExit(this);
        _currentState = state;
        _currentState?.OnEnter(this);
    }

    /// <summary>
    /// Controls the bot's animation variables.
    /// </summary>
    /// <param name="botState"></param>
    public void ForceControlBotAnimation(BotState botState)
    {
        switch (botState)
        {
            case BotState.Idle:
                isIdle = true;
                isAttack = false;
                isDead = false;
                break;
            case BotState.Move:
                isIdle = false;
                break;
            case BotState.Attack:
                isIdle = true;
                break;
            case BotState.Dead:
                _navigationIndicator?.ReturnToPool();
                _navigationIndicator = null;
                target = null;
                targetsList.Clear();
                ReturnToPool();
                break;
            default:
                isIdle = true;
                isAttack = false;
                isUlti = false;
                isDance = false;
                isDead = false;
                isWin = false;
                break;
        }
        _botState = botState;
        SetAnimationParameters();
    }

    /// <summary>
    /// Randomly sets the bot's move direction.
    /// </summary>
    public void SetMoveDirection()
    {
        direction = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            0f,
            UnityEngine.Random.Range(-1f, 1f))
            .normalized;
    }

    /// <summary>
    /// Called when radar detects a wall.
    /// </summary>
    /// <param name="wallPosition"></param>
    private void OnWallDetected(Vector3 wallPosition)
    {
        Vector3 reflectedDirection = m_transform.position - wallPosition;

        direction = reflectedDirection.normalized;
        Quaternion rotationQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-80f, 81f), Vector3.up);

        // Rotate the input vector
        direction = (rotationQuaternion * reflectedDirection).normalized;
    }

    /// <summary>
    /// Called when radar detects a bullet.
    /// </summary>
    /// <param name="weaponBase"></param>
    public void OnBulletDetected(ThrowWeapon weaponBase)
    {
        if (weaponBase.Attacker == this) return;
        byte randomChance = (byte)UnityEngine.Random.Range(0, 101);
        if (_botDogdeChance > randomChance)
        {
            Vector3 incomingBulletDirection = weaponBase.MoveDirection;
            Quaternion rotationQuaternion = Quaternion.AngleAxis((UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(60f, 120f), Vector3.up);
            direction = (rotationQuaternion * incomingBulletDirection).normalized;
            SetState(new DodgeState());
        }
    }

    /// <summary>
    /// OnDead is called when the bot hited by a weapon.
    /// </summary>
    public override void OnDead()
    {
        SetState(new DeadState());
        base.OnDead();
    }
    #endregion
}
