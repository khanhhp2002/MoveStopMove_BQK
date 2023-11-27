using System;
using UnityEngine;

public class Bot : CharacterBase, IPoolable<Bot>
{
    [Header("Bot Components"), Space(5f)]
    [SerializeField, Range(0.1f, 0.5f)] private float _navigationIndicatorSpeed;
    [SerializeField] private float _navigationIndicatorRange;

    [Header("Bot Stats"), Space(5f)]
    [SerializeField] private BotState _botState;
    [SerializeField, Range(0f, 100f)] private byte _botDogdeChance;

    private Vector2 _botPos;
    private Vector2 _playerPos;
    private Vector2 _directionToPlayer;
    private Vector2 _indicatorPos;
    private NavigationIndicator _navigationIndicator;

    private Action<Bot> _returnPoolAction;

    private IState _currentState;

    private bool _isIgnoreAttack = false;

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

    private void ResetIgnoreAttack()
    {
        _isIgnoreAttack = false;
    }

    public CharacterBase Target => target;

    protected override void OnEnable()
    {
        weaponData = WeaponManager.Instance.GetRandomWeaponData();
        EquipWeapon(weaponData);
        pantSkin.material = GameplayManager.Instance.GetPantByIndex();
        _currentState = new IdleState();
        base.OnEnable();
    }

    protected override void Start()
    {
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
    protected override void Update()
    {
        switch (GameplayManager.Instance.GameState)
        {
            case GameState.Preparing:
                break;
            case GameState.Playing:
                base.Update();
                NavigationIndicatorControl();
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

        _botPos = Camera.main.WorldToScreenPoint(transform.position);

        if (Vector3.Dot(this.transform.position - Camera.main.transform.position, Camera.main.transform.forward) < 0) // Behind the camera?
        {
            _botPos.x = Screen.width - _botPos.x;
            _botPos.y = Screen.height - _botPos.y;
        }

        if (_botPos.x < -10f || _botPos.x > Screen.width + 10f || _botPos.y < -10f || _botPos.y > Screen.height + 10f) // Out of screen
        {
            _playerPos = Camera.main.WorldToScreenPoint(GameplayManager.Instance.Player.transform.position);
            _directionToPlayer = (_botPos - _playerPos).normalized;
            _indicatorPos = _directionToPlayer * _navigationIndicatorRange + _playerPos;

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
            if (_navigationIndicator is not null)
            {
                _navigationIndicator.ReturnToPool();
                _navigationIndicator = null;
            }
        }
    }

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
        Vector3 reflectedDirection = transform.position - wallPosition;

        direction = reflectedDirection.normalized;
        Quaternion rotationQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-80f, 81f), Vector3.up);

        // Rotate the input vector
        direction = (rotationQuaternion * reflectedDirection).normalized;
    }

    public void OnBulletDetected(ThrowWeapon weaponBase)
    {
        if (weaponBase.Attacker == this) return;
        byte randomChance = (byte)UnityEngine.Random.Range(0, 100);
        if (_botDogdeChance > randomChance)
        {
            Vector3 incomingBulletDirection = weaponBase.MoveDirection;
            direction = (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * Vector3.Cross(incomingBulletDirection, Vector3.up);
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
}
