using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Components"), Space(5f)]
    [SerializeField] private Collider _collider;

    [Header("Weapon Stats"), Space(5f)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _range;

    private bool _hasDestination = false;
    private Vector3 _destination;
    private Vector3 _originPoint;
    private float _distance;
    private float _remainingDistance;

    public Collider Collider => _collider;

    // Start is called before the first frame update
    private void Start()
    {
        _originPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hasDestination) return;

        transform.Rotate(Vector3.back, _rotateSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(_originPoint, _destination, 1 - (_remainingDistance / _distance));

        _remainingDistance -= _moveSpeed * Time.fixedDeltaTime;

        if (transform.position == _destination)
        {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Set the destination of the weapon that will be throwed to calculate the trajectory
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    public void SetDestination(Vector3 origin, Vector3 direction)
    {
        _destination = origin + direction * _range;
        _destination.y = transform.position.y;
        _remainingDistance = _distance = Vector3.Distance(origin, _destination);
        _hasDestination = true;
        _collider.enabled = true;
    }
}
