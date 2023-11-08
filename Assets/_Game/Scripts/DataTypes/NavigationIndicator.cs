using System;
using TMPro;
using UnityEngine;

public class NavigationIndicator : MonoBehaviour, IPoolable<NavigationIndicator>
{
    [Header("Navigation Indicator Components"), Space(5f)]
    [SerializeField] private GameObject _pointer;
    [SerializeField] private TMP_Text _pointText;

    [Header("Navigation Indicator Stats"), Space(5f)]
    [SerializeField] private float _pointerRadius;
    private Action<NavigationIndicator> _returnAction;

    /// <summary>
    /// Initializes the navigation indicator.
    /// </summary>
    /// <param name="returnAction"></param>
    public void Initialize(Action<NavigationIndicator> returnAction)
    {
        _returnAction = returnAction;
    }

    /// <summary>
    /// Returns the navigation indicator to the pool.
    /// </summary>
    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }

    public void LookAt(Vector2 direction)
    {
        _pointer.transform.localPosition = direction.normalized * _pointerRadius;

        float angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        _pointer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetPoint(int value)
    {
        _pointText.text = value.ToString();
    }
}
