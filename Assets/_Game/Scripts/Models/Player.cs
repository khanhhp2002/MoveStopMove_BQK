using UnityEngine;

public class Player : CharacterBase
{
    private float _horizontal;
    private float _vertical;
    private Vector3 _direction;
    private bool _inAttackProcess = false;

    private void FixedUpdate()
    {
        PlayerInput();
        Movement();
        Attack();
        SetAnimationParameters();
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
    /// Controls the attack of the character.
    /// </summary>
    private void Attack()
    {
        if (!_isAttack || _inAttackProcess || _isDead || _isWin || _isUlti) return;
        _inAttackProcess = true;
        Invoke(nameof(EndAttackProcess), .65f);
    }


    /// <summary>
    /// Sets the animation parameters.
    /// </summary>
    private void SetAnimationParameters()
    {
        _animator.SetBool(IDLE_ANIMATION, _isIdle);
        _animator.SetBool(WIN_ANIMATION, _isWin);
        _animator.SetBool(DEAD_ANIMATION, _isDead);
        _animator.SetBool(DANCE_ANIMATION, _isDance);
        _animator.SetBool(ULTI_ANIMATION, _isUlti);
        _animator.SetBool(ATTACK_ANIMATION, _isAttack);
    }

    /// <summary>
    /// The end of the attack process.
    /// </summary>
    private void EndAttackProcess()
    {
        _inAttackProcess = false;
        _isAttack = false;
    }
}
