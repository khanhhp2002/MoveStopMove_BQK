using Cinemachine;
using UnityEngine;

public class Player : CharacterBase
{
    private float _horizontal;
    private float _vertical;
    private Vector3 _direction;

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
        PlayerInput();
        Movement();
        base.FixedUpdate();
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
    /// Controls the movement of the character.
    /// </summary>
    private void Movement()
    {
        if (_isAttack || _isIdle || _isWin || _isDead) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotateSpeed);
        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, _moveSpeed * Time.fixedDeltaTime);
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
