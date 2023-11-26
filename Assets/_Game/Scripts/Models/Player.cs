using UnityEngine;

public class Player : CharacterBase
{
    private float _horizontal;
    private float _vertical;
    [SerializeField] private Transform _targetLock;
    [SerializeField] private float _targetLockSpeed;
    [SerializeField] private float _groundOffset;
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
    protected override void Update()
    {
        switch (GameplayManager.Instance.GameState)
        {
            case GameState.Preparing:
                break;
            case GameState.Playing:
                base.Update();
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
            WeaponBase weapon = GameObject.Instantiate(weaponData.WeaponPrefab);
            weapon.Throw(weaponHolder.position, direction, attackRange, scaleValue, this, OnGetKill, weaponData);
        }
    }

    private void LockTarget()
    {
        if (target is null)
        {
            _targetLock.gameObject.SetActive(false);
            _targetLock.position = Vector3.Lerp(_targetLock.position, this.transform.position, _targetLockSpeed);
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
}
