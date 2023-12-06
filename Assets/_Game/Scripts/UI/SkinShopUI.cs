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
    [SerializeField] private Button _itemPrefab;
    [SerializeField] private Button _purchaseItem;
    [SerializeField] private Button _purchaseItemWithAds;
    [SerializeField] private Button _useWeapon;

    [Header("Weapon-Display Settings"), Space(5f)]
    [SerializeField] private Transform _itemContainer;

    [Header("Button-Display Colors"), Space(5f)]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;

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
        ResetItem();
        SpawnItem();
    }

    private void PantsShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        ResetItem();
        SpawnItem();
    }

    private void WeaponShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        ResetItem();
        SpawnItem();
    }

    private void FullSetShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        ResetItem();
        SpawnItem();
    }

    private void ResetItem()
    {
        if (_itemContainer.childCount is 0) return;
        foreach (Transform child in _itemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnItem()
    {
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            Button item = Instantiate(_itemPrefab, _itemContainer);
        }
    }
}
