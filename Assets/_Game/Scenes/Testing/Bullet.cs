using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Stack<Bullet> _bullets = new Stack<Bullet>();
    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(Move());
    }
    void Update()
    {

    }

    private IEnumerator Move()
    {
        float time = 0f;
        while (time < 1f)
        {
            Vector3 center = (Movement.Instance._aPoint.position + Movement.Instance._bPoint.position) / 2f;
            transform.position = Vector3.Slerp(Movement.Instance._aPoint.position - center, Movement.Instance._bPoint.position - center, time) + center;
            time += Time.deltaTime;
            yield return null;
        }
        _bullets.Push(this);
        gameObject.SetActive(false);
    }
}
