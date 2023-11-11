using System;
using UnityEngine;

public class Bot : CharacterBase, IPoolable<Bot>
{
    [SerializeField] private float _navigationIndicatorRange;
    [SerializeField, Range(0.1f, 0.5f)] private float _navigationIndicatorSpeed;
    [SerializeField] private BotState _botState;
    [SerializeField] private Vector2 _botPos;
    [SerializeField] private Vector2 _playerPos;
    [SerializeField] private Vector2 _directionToPlayer;
    [SerializeField] private Vector2 _indicatorPos;
    private NavigationIndicator _navigationIndicator;

    private Action<Bot> _returnPoolAction;

    private IState _currentState;

    protected void OnEnable()
    {
        _currentState = new IdleState();
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
                NavigationIndicatorControl();

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
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (_isDead) SetState(new DeadState());
    }

    /// <summary>
    /// Controls the indicator that shows the bot's position when it is out of the screen.
    /// </summary>
    private void NavigationIndicatorControl()
    {
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
                _navigationIndicator.SetPoint(_killCount);
            }
            else
            {
                _navigationIndicator.transform.position = Vector2.Lerp(_navigationIndicator.transform.position, _indicatorPos, _navigationIndicatorSpeed);
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
            _navigationIndicator.SetPoint(_killCount);
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
                _isIdle = true;
                _isAttack = false;
                break;
            case BotState.Move:
                _isIdle = false;
                _isAttack = false;
                break;
            case BotState.Attack:
                _isIdle = true;
                _isAttack = true;
                break;
            case BotState.Dead:
                // I don't know what to do here because the "_isDead" variable is already set to true by OnTriggerEnter method. φ(*￣0￣)
                _navigationIndicator?.ReturnToPool();
                _navigationIndicator = null;
                break;
            default:
                _isIdle = true;
                _isAttack = false;
                _isUlti = false;
                _isDance = false;
                _isDead = false;
                _isWin = false;
                break;
        }
        _botState = botState;
    }

    /// <summary>
    /// Randomly sets the bot's move direction.
    /// </summary>
    public void SetMoveDirection()
    {
        _direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}
