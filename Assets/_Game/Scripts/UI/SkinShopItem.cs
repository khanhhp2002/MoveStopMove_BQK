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
    private int price;


    private Action<SkinShopItem> _returnAction;

    public int ItemIndex => _itemIndex;
    public int Price => price;
    public void Initialize(Action<SkinShopItem> returnAction)
    {
        this._returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }

    public void SetItem(Sprite sprite, int itemIndex, int price, bool isPurchased, Action<int, SkinShopItem> OnClickItem)
    {
        _image.sprite = sprite;
        _itemIndex = itemIndex;
        this.price = price;
        _lock.SetActive(!isPurchased);
        _aspectRatioFitter.aspectRatio = sprite.textureRect.width / sprite.textureRect.height;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            OnClickItem?.Invoke(_itemIndex, this);
        });
    }

    public void SetUnlock()
    {
        _lock.SetActive(false);
    }
}
