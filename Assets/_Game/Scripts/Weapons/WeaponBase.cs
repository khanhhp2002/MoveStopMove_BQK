using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Components"), Space(5f)]
    [SerializeField] private Collider _collider;

    [Header("Weapon Stats"), Space(5f)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _range;
    [SerializeField] private float _customDestroyTime;
    [SerializeField] private bool _piercingAble;

    private bool _hasDestination = false;
    private Vector3 _destination;
    private Vector3 _originPoint;
    private float _distance;
    private float _remainingDistance;
    private Action<CharacterBase> _onGetKill;
    private CharacterBase _caster;

    public CharacterBase Caster => _caster;

    /// <summary>
    /// Get the collider of the weapon.
    /// </summary>
    public Collider Collider => _collider;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _originPoint = transform.position;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void FixedUpdate()
    {
        if (!_hasDestination || GameplayManager.Instance.GameState == GameState.Paused) return;

        transform.Rotate(Vector3.back, _rotateSpeed * Time.fixedDeltaTime);

        transform.position = Vector3.Lerp(
            _originPoint,
            _destination,
            1 - (_remainingDistance / _distance));

        _remainingDistance -= _moveSpeed * Time.fixedDeltaTime;

        if (transform.position == _destination)
        {
            Destroy(gameObject, _customDestroyTime);
        }

    }

    /// <summary>
    /// Set the destination of the weapon that will be throwed to calculate the trajectory
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    public void SetDestination(Vector3 origin, Vector3 direction, Action<CharacterBase> onHit, CharacterBase caster)
    {
        _destination = origin + direction * _range;
        _destination.y = transform.position.y;
        _remainingDistance = _distance = Vector3.Distance(origin, _destination);
        _hasDestination = true;
        _onGetKill = onHit;
        _caster = caster;
        _collider.enabled = true;
    }

    /// <summary>
    /// Called when the weapon hit character.
    /// </summary>
    public void OnHit(CharacterBase identity)
    {
        _onGetKill?.Invoke(identity);

        if (_piercingAble) return;

        _collider.enabled = false;
        Destroy(gameObject, _customDestroyTime);
    }
}
