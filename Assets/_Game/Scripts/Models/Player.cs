using UnityEngine;

public class Player : CharacterBase
{
    private void FixedUpdate()
    {
        Movement();
    }
    /// <summary>
    /// Controls the movement of the character.
    /// </summary>
    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction == Vector3.zero)
        {
            _animator.SetBool("IsIdle", true);
            transform.Translate(Vector3.zero);
        }
        else
        {
            _animator.SetBool("IsIdle", false);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotateSpeed);
            transform.position = Vector3.Lerp(transform.position, transform.position + direction.normalized, _moveSpeed * Time.fixedDeltaTime);
        }
    }


}
