using UnityEngine;

public class Player : CharacterBase
{
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction == Vector3.zero)
        {
            //_rigidbody.velocity = Vector3.zero;
            _animator.SetBool("IsIdle", true);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
            _animator.SetBool("IsIdle", false);
        }
    }
}
