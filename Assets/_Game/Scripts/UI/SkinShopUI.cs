using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : UIBase<SkinShopUI>
{
    [Header("Action-Button"), Space(5f)]
    [SerializeField] private Button _exitShop;
    [SerializeField] private Button _helmetShopBtn;
    [SerializeField] private Button _pantsShopBtn;
    [SerializeField] private Button _weaponShopBtn;
    [SerializeField] private Button _fullSetShopBtn;
    [SerializeField] private Button _purchaseItem;
    [SerializeField] private Button _purchaseItemWithAds;
    [SerializeField] private Button _useWeapon;

    [Header("Weapon-Display Settings"), Space(5f)]
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private ObjectPool<SkinShopItem> _itemPool;
    private List<SkinShopItem> _activeList = new List<SkinShopItem>();

    private void Awake()
    {
        _itemPool = new ObjectPool<SkinShopItem>(_itemPrefab);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _exitShop.onClick.AddListener(ExitShop);
        _helmetShopBtn.onClick.AddListener(HelmetShop);
        _pantsShopBtn.onClick.AddListener(PantsShop);
        _weaponShopBtn.onClick.AddListener(WeaponShop);
        _fullSetShopBtn.onClick.AddListener(FullSetShop);
        _helmetShopBtn.Select();
        HelmetShop();
    }

    /// <summary>
    /// Exit shop.
    /// </summary>
    private void ExitShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenMenuUI();
    }

    private void HelmetShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem();
    }

    private void PantsShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem();
    }

    private void WeaponShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem();
    }

    private void FullSetShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem();
    }

    private void SpawnItem()
    {
        scrollRect.normalizedPosition = Vector3.zero;
        int currentItemCount = Random.Range(5, 10);
        int offsetCount = _activeList.Count - currentItemCount;

        if (_activeList.Count > currentItemCount)
        {
            for (int i = 0; i < offsetCount; i++)
            {
                SkinShopItem item = _activeList[0];
                _activeList.RemoveAt(0);
                item.ReturnToPool();
            }
        }
        else if (_activeList.Count < currentItemCount)
        {
            for (int i = _activeList.Count; i < currentItemCount; i++)
            {
                SkinShopItem item = _itemPool.Pull(_itemContainer);
                _activeList.Add(item);
            }
        }
    }
}
