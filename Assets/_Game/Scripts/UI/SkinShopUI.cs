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
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Color32 _selectedColor;
    [SerializeField] private Color32 _unselectedColor;

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
        _pantsShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _helmetShopBtn.image.color = _selectedColor;
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem(GameplayManager.Instance.SkinSO.Hairs.Count);
    }

    private void PantsShop()
    {
        _helmetShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _selectedColor;
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem(GameplayManager.Instance.SkinSO.Pants.Count);
    }

    private void WeaponShop()
    {
        _helmetShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _selectedColor;
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem(Random.Range(5, 10));
    }

    private void FullSetShop()
    {
        _helmetShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _selectedColor;
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SpawnItem(Random.Range(5, 10));
    }

    private void SpawnItem(int numberOfItem)
    {
        _scrollRect.normalizedPosition = Vector3.zero;
        int offsetCount = _activeList.Count - numberOfItem;

        if (_activeList.Count > numberOfItem)
        {
            for (int i = 0; i < offsetCount; i++)
            {
                SkinShopItem item = _activeList[0];
                _activeList.RemoveAt(0);
                item.ReturnToPool();
            }
        }
        else if (_activeList.Count < numberOfItem)
        {
            for (int i = _activeList.Count; i < numberOfItem; i++)
            {
                SkinShopItem item = _itemPool.Pull(_itemContainer);
                _activeList.Add(item);
            }
        }
    }
}
