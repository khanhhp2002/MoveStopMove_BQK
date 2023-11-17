using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Singleton<Movement>
{
    public Transform _aPoint;
    public Transform _bPoint;
    public float _offset;
    public Bullet _bulletPrefab;

    public float time;
    private float count;

    private void Update()
    {
        if (count > 0)
        {
            count -= Time.deltaTime;
            return;
        }
        count += time;

        if (Bullet._bullets.Count > 0)
        {
            var tmp = Bullet._bullets.Pop().gameObject;
            tmp.SetActive(true);
            tmp.transform.position = _aPoint.position;
        }
        else
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _aPoint.position;
        }

    }
}
