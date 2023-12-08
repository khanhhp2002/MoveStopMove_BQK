using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CharacterBase : MonoBehaviour
{
    #region Fields
    [Header("Character Components"), Space(5f)]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Transform hairContainer;
    [SerializeField] protected SkinnedMeshRenderer pantSkin;
    [SerializeField] protected SkinnedMeshRenderer skinColor;
    [SerializeField] protected Rigidbody m_rigidbody;
    [SerializeField] protected Collider hitCollider;
    [SerializeField] protected RadarController radarController;
    [SerializeField] protected Canvas infoCanvas;
    [SerializeField] protected TMP_Text characterName;
    [SerializeField] protected TMP_Text characterPoint;

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

    // Current weapon data.
    protected WeaponData weaponData;

    // Targets in range.
    protected CharacterBase target;
    protected List<CharacterBase> targetsList = new List<CharacterBase>();

    // Character default stats.
    protected int point = 1;
    protected int killCount = 0;
    protected float scaleValue = 1;
    protected float attackTimer = 0f;

    // Event.
    protected Action<CharacterBase> OnDeadCallBack;

    // Cached variables.
    protected Transform m_transform;
    protected Vector3 direction = Vector3.zero;
    protected GameObject m_hair;
    protected GameObject m_weapon;
    #endregion

    #region properties
    protected bool cantAttack => !target || !isIdle || isDead || isWin || isUlti;
    protected bool cantMove => isIdle || isWin || isDead;
    public int Point => point;
    public bool IsDead => isDead;
    public int KillCount => killCount;
    public string CharacterName
    {
        get => characterName.text;
        set => characterName.text = value;
    }
    #endregion

    #region Methods
    protected virtual void OnEnable()
    {
        infoCanvas.gameObject.SetActive(false);
        radarController.gameObject.SetActive(true);
        characterPoint.text = point.ToString();
        skinColor.material = RuntimeData.Instance.SkinStorage.SkinColors[UnityEngine.Random.Range(0, RuntimeData.Instance.SkinStorage.SkinColors.Count)];
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        m_transform = this.transform;
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
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void LateUpdate()
    {
        CanvasController();
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (m_weapon is not null)
        {
            Destroy(m_weapon.gameObject);
        }
        this.weaponData = weaponData;
        m_weapon = Instantiate(this.weaponData.WeaponModel, weaponHolder);
        m_weapon.transform.localScale = this.weaponData.HandWeaponScale * Vector3.one;
        m_weapon.transform.localPosition = this.weaponData.HandWeaponOffset;
        m_weapon.transform.localRotation = Quaternion.identity;
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

        m_transform.rotation = Quaternion.Slerp(
            m_transform.rotation,
            Quaternion.LookRotation(direction == Vector3.zero ? m_transform.forward : direction),
            rotateSpeed * Time.fixedDeltaTime);


        m_transform.position = Vector3.Lerp(
            m_transform.position,
            m_transform.position + direction,
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
        m_transform.rotation = Quaternion.Slerp(
            m_transform.rotation,
            Quaternion.LookRotation(target.transform.position - m_transform.position),
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
            direction = m_transform.forward;
        else
            direction = (target.transform.position - this.weaponHolder.position).normalized;

        direction.y = 0f;

        WeaponManager.Instance.GetWeapon(weaponData.WeaponType, weaponData.ThrowWeaponPrefab)
            .Throw(weaponHolder.position, direction, attackRange, scaleValue, this, weaponData, OnGetKill);

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

    /// <summary>
    /// Called when the weapon that the character throws hits another character.
    /// </summary>
    protected virtual void OnGetKill(CharacterBase target)
    {
        Player player = target as Player;
        if (player != null)
            player.KillerName = CharacterName;
        if (isDead) return;
        GameplayManager.Instance.AliveCounter--;
        killCount++;
        point += target.Point;
        characterPoint.text = point.ToString();
        scaleValue = (maxLocalScale - localScaleIncreaseValue / (localScaleIncreaseValue + point));
        m_transform.localScale = scaleValue * Vector3.one;
        m_transform.DOScale(scaleValue, 0.5f);
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

    /// <summary>
    /// Called when the character dies.
    /// </summary>
    public virtual void OnDead()
    {
        isDead = true;
        SetAnimationParameters();
        radarController.gameObject.SetActive(false);
        target = null;
        targetsList.Clear();
        OnDeadCallBack?.Invoke(this);
    }
    #endregion
}
