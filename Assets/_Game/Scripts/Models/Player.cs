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
        _weaponData = WeaponManager.Instance.GetWeaponDataByIndex(GameplayManager.Instance._userData.EquippedWeapon);
        EquipWeapon(_weaponData);
        _pantSkin.material = GameplayManager.Instance.GetPantByIndex(GameplayManager.Instance._userData.EquippedPant);
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
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _direction = new Vector3(_horizontal, 0f, _vertical).normalized;
        _isIdle = _direction == Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            _weaponData.Throw(_weaponHolder.position, this.transform.forward, _attackRange, this, OnGetKill, _hitCollider);
        }
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        base.OnTriggerEnter(other);
    }
}
