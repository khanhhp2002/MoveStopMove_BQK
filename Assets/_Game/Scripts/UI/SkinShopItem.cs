using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour, IPoolable<SkinShopItem>
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _lock;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    private int _itemIndex;


    private Action<SkinShopItem> _returnAction;

    public int ItemIndex => _itemIndex;
    public void Initialize(Action<SkinShopItem> returnAction)
    {
        this._returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }

    public void SetItem(Sprite sprite, int itemIndex, bool isPurchased)
    {
        _image.sprite = sprite;
        _itemIndex = itemIndex;
        _lock.SetActive(!isPurchased);
        _aspectRatioFitter.aspectRatio = sprite.textureRect.width / sprite.textureRect.height;
        //sprite.textureRect.width
    }


}
