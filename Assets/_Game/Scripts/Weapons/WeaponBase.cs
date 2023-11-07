using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Stats"), Space(5f)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _range;

    private Vector3? _destination = null;
    private Vector3 _originPoint;
    private float _progress;

    // Start is called before the first frame update
    private void Start()
    {
        _originPoint = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_destination.HasValue) return;

        Debug.Log(_destination.Value);
        // Rotate the object around its local Z axis at _rotateSpeed degrees per second
        transform.Rotate(Vector3.back, _rotateSpeed * Time.deltaTime);
        // Calculate the lerp value.
        _progress += _moveSpeed * Time.deltaTime;

        // Move the object to the lerped position.
        transform.position = Vector3.Lerp(_originPoint, _destination.Value, _progress);

        // If the object has reached the end point, stop moving it.
        if (transform.position == _destination)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestination(Vector3 origin, Vector3 direction)
    {
        _destination = origin + direction * _range;
    }
}
