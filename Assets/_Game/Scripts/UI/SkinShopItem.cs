using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour, IPoolable<SkinShopItem>
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    private Action<SkinShopItem> _returnAction;
    public void Initialize(Action<SkinShopItem> returnAction)
    {
        this._returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }
}
