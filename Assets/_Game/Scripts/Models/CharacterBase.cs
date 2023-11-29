using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Rigidbody m_rigidbody;
    [SerializeField] protected Collider hitCollider;
    [SerializeField] protected RadarController radarController;
    //[SerializeField] protected WeaponBase weapon;
    [SerializeField] protected Canvas infoCanvas;
    [SerializeField] protected SkinnedMeshRenderer pantSkin;

    [Header("Character Stats"), Space(5f)]
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxLocalScale;
    [SerializeField] protected float localScaleIncreaseValue;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;

    // Animation name constants.
    protected const string IDLE_ANIMATION = "IsIdle";
    protected const string WIN_ANIMATION = "IsWin";
    protected const string ATTACK_ANIMATION = "IsAttack";
    protected const string DEAD_ANIMATION = "IsDead";
    protected const string DANCE_ANIMATION = "IsDance";
    protected const string ULTI_ANIMATION = "IsUlti";

    // Animation parameters.
    protected bool isIdle = true;
    protected bool isDead = false;
    protected bool isWin = false;
    protected bool isAttack = false;
    protected bool isDance = false;
    protected bool isUlti = false;
    protected bool isAttacked = false;

    protected bool cantMove => isIdle || isWin || isDead;
    protected bool cantAttack => !target || !isIdle || isDead || isWin || isUlti;
    // Current weapon data.
    protected WeaponData weaponData;

    // Targets in range.
    protected CharacterBase target;
    protected List<CharacterBase> targetsList = new List<CharacterBase>();

    protected int point = 1;
    public int Point => point;
    protected float scaleValue = 1;
    protected float attackTimer = 0f;
    protected Vector3 direction = Vector3.zero;

    protected Action<CharacterBase> OnDeadCallBack;

    /// <summary>
    /// Subscribes to the OnDeadCallBack event.
    /// </summary>
    /// <param name="callBack"></param>
    public void SubcribeOnDeadCallBack(Action<CharacterBase> callBack)
    {
        OnDeadCallBack += callBack;
    }

    /// <summary>
    /// Unsubscribes from the OnDeadCallBack event.
    /// </summary>
    /// <param name="callBack"></param>
    public void UnsubcribeOnDeadCallBack(Action<CharacterBase> callBack)
    {
        OnDeadCallBack -= callBack;
    }

    public bool IsDead => isDead;

    protected virtual void OnEnable()
    {
        infoCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        radarController.OnEnemyEnterCallBack(OnFoundTarget);
        radarController.OnEnemyExitCallBack(OnLostTarget);
    }

    /// <summary>
    /// FixedUpdate is called once per frame.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        Movement();
        Attack();
        //SetAnimationParameters();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        CanvasController();
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (weaponHolder.childCount > 0)
        {
            Destroy(weaponHolder.GetChild(0).gameObject);
        }
        this.weaponData = weaponData;
        var weapon = Instantiate(this.weaponData.WeaponModel, weaponHolder);
        weapon.transform.localScale = this.weaponData.HandWeaponScale * Vector3.one;
        weapon.transform.localPosition = this.weaponData.HandWeaponOffset;
        weapon.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Controls the movement of the character.
    /// </summary>
    private void Movement()
    {
        //_isAttack || 
        if (cantMove) return;

        // cancel attack
        if (isAttack)
        {
            isUlti = false;
            isAttack = false;
            if (!isAttacked) attackTimer = 0f;
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            rotateSpeed * Time.fixedDeltaTime);


        transform.position = Vector3.Lerp(
            transform.position,
            transform.position + direction,
            moveSpeed * Time.fixedDeltaTime);

        SetAnimationParameters();
    }

    /// <summary>
    /// Controls the attack of the character.
    /// </summary>
    private void Attack()
    {
        // Cooldown
        if (attackTimer > 0f)
        {
            attackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            isUlti = false;
            isAttack = false;
            isAttacked = false;
        }

        // Requirements conditions
        if (cantAttack) return;

        // Look at the target
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(target.transform.position - transform.position),
            rotateSpeed);

        // Can attack
        if (!isAttack && attackTimer <= 0)
        {
            isAttack = true;
            if (weaponData.WeaponType == WeaponType.Boomerang) isUlti = true;
            attackTimer += weaponData.BonusAttackSpeed;
        }

        SetAnimationParameters();
    }

    /// <summary>
    /// ThrowWeapon is called when the event in attack/ulti is triggered.
    /// </summary>
    private void ThrowWeapon()
    {
        // cancel attack
        if (!isAttack)
        {
            isUlti = false;
            isAttack = false;
            SetAnimationParameters();
            return;
        }
        Vector3 direction;
        if (target is null)
            direction = transform.forward;
        else
            direction = (target.transform.position - this.weaponHolder.position).normalized;

        direction.y = 0f;

        //ThrowWeapon weapon = GameObject.Instantiate(weaponData.ThrowWeaponPrefab);
        //weapon.Throw(weaponHolder.position, direction, attackRange, scaleValue, this, weaponData, OnGetKill);
        WeaponManager.Instance.Throw(weaponHolder.position, direction, attackRange, scaleValue, this, weaponData, OnGetKill, weaponData.WeaponType, weaponData.ThrowWeaponPrefab);

        isAttacked = true;
    }

    /// <summary>
    /// The end of the attack process.
    /// </summary>
    private void EndAttackProcess()
    {
        isUlti = false;
        isAttack = false;
        isAttacked = false;
        SetAnimationParameters();
    }

    /// <summary>
    /// Sets the animation parameters.
    /// </summary>
    protected void SetAnimationParameters()
    {
        animator.SetBool(DEAD_ANIMATION, isDead);
        animator.SetBool(WIN_ANIMATION, isWin);
        animator.SetBool(DANCE_ANIMATION, isDance);
        animator.SetBool(ULTI_ANIMATION, isUlti);
        animator.SetBool(ATTACK_ANIMATION, isAttack);
        animator.SetBool(IDLE_ANIMATION, isIdle);
    }

    /// <summary>
    /// Make the canvas look at the camera.
    /// </summary>
    private void CanvasController()
    {
        infoCanvas.gameObject.SetActive(true);
        infoCanvas.transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// Called when the weapon that the character throws hits another character.
    /// </summary>
    protected virtual void OnGetKill(CharacterBase target)
    {
        if (isDead) return;

        point += target.Point;
        scaleValue = (maxLocalScale - localScaleIncreaseValue / (localScaleIncreaseValue + point));
        transform.localScale = scaleValue * Vector3.one;
    }

    /// <summary>
    /// OnFoundTarget is called when the radar detects an enemy.
    /// </summary>
    /// <param name="target"></param>
    protected void OnFoundTarget(CharacterBase target)
    {
        if (target == this || target.IsDead) return;

        target.SubcribeOnDeadCallBack(OnLostTarget);

        if (this.target is null)
        {
            this.target = target;
        }
        else
        {
            targetsList.Add(target);
        }
    }

    /// <summary>
    /// OnLostTarget is called when the radar loses an enemy.
    /// </summary>
    /// <param name="target"></param>
    protected void OnLostTarget(CharacterBase target)
    {
        target.UnsubcribeOnDeadCallBack(OnLostTarget);

        if (this.target == target)
        {
            this.target = null;
            if (targetsList.Count > 0)
            {
                this.target = targetsList[0];
                targetsList.RemoveAt(0);
            }
        }
        else
        {
            targetsList.Remove(target);
        }
    }

    public virtual void OnDead()
    {
        isDead = true;
        SetAnimationParameters();
        OnDeadCallBack?.Invoke(this);
    }
}
