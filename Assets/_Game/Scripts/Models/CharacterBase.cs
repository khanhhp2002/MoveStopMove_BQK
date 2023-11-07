using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _weaponHolder;
    [SerializeField] protected float _speed = 3f;
    [SerializeField] protected Rigidbody _rigidbody;

    protected bool _isIdle = true;
    protected bool _isDead = false;
    protected bool _isWin = false;
    protected bool _isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        //_animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
