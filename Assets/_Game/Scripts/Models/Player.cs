using UnityEngine;

public class Player : CharacterBase
{
    private float _horizontal;
    private float _vertical;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected override void Start()
    {
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
                _isIdle = true;
                break;
            case GameState.Playing:
                PlayerInput();
                base.FixedUpdate();
                break;
            case GameState.Paused:
                _animator.speed = 0f;
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
        if (!_inAttackProcess)
        {
            _isAttack = Input.GetMouseButtonDown(0);
        }
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _direction = new Vector3(_horizontal, 0f, _vertical).normalized;
        _isIdle = _direction == Vector3.zero;
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
