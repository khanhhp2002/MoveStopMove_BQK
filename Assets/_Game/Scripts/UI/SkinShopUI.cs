using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : UIBase<SkinShopUI>
{
    [Header("Action-Button"), Space(5f)]
    [SerializeField] private Button _exitShop;
    [SerializeField] private Button _hairShopBtn;
    [SerializeField] private Button _pantsShopBtn;
    [SerializeField] private Button _weaponShopBtn;
    [SerializeField] private Button _fullSetShopBtn;
    [SerializeField] private Button _purchaseItem;
    [SerializeField] private Button _purchaseItemWithAds;
    [SerializeField] private Button _equipItem;

    [Header("Display Settings"), Space(5f)]
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Color32 _selectedColor;
    [SerializeField] private Color32 _unselectedColor;

    [Header("Other Components"), Space(5f)]
    [SerializeField] private GameObject _loadingImage;

    private ObjectPool<SkinShopItem> _itemPool;
    private List<SkinShopItem> _activeList = new List<SkinShopItem>();
    private Tween _loadingTween;
    private ShopType _currentShopType;
    private byte _currentItemIndex;
    private SkinShopItem _currentItem;

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        HairShop();
        _currentShopType = ShopType.Hair;
        _currentItemIndex = 0;
        OnClickItem(_currentItemIndex, null);
        ((Player)GameplayManager.Instance.Player).IsDance = true;
    }

    /// <summary>
    /// OnDisable is called when the object becomes disabled.
    /// </summary>
    private void OnDisable()
    {
        ((Player)GameplayManager.Instance.Player).IsDance = false;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _itemPool = new ObjectPool<SkinShopItem>(_itemPrefab);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _exitShop.onClick.AddListener(() =>
        {
            ExitShop();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _hairShopBtn.onClick.AddListener(() =>
        {
            HairShop();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _pantsShopBtn.onClick.AddListener(() =>
        {
            PantsShop();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _weaponShopBtn.onClick.AddListener(() =>
        {
            WeaponShop();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _fullSetShopBtn.onClick.AddListener(() =>
        {
            FullSetShop();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _purchaseItem.onClick.AddListener(() =>
        {
            PurchaseItem();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _purchaseItemWithAds.onClick.AddListener(() =>
        {
            PurchaseItemWithAds();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });

        _equipItem.onClick.AddListener(() =>
        {
            UseItem();
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        });
    }

    /// <summary>
    /// Exit shop.
    /// </summary>
    private void ExitShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenMenuUI();
    }

    private void HairShop()
    {
        _pantsShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _hairShopBtn.image.color = _selectedColor;
        ResetShop();
        StartCoroutine(LoadHairItem());
    }

    private IEnumerator LoadHairItem()
    {
        canvasGroup.blocksRaycasts = false;
        _loadingImage.SetActive(true);
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        int condition = RuntimeData.Instance.SkinStorage.Hairs.Count;
        for (int i = 0; i < condition; i++)
        {
            SkinShopItem item = _itemPool.Pull(_itemContainer);
            HairData hairData = RuntimeData.Instance.SkinStorage.Hairs[i];
            item.SetItem(hairData.Sprite, i, hairData.Price, GameplayManager.Instance.UserData.UnlockedHairs.Contains((byte)i), OnClickItem);
            _activeList.Add(item);
            item.transform.SetAsLastSibling();
            yield return null;
        }
        _loadingTween.Kill();
        _loadingImage.SetActive(false);
        canvasGroup.blocksRaycasts = true;
        _currentShopType = ShopType.Hair;
    }

    private void PantsShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _selectedColor;
        ResetShop();
        StartCoroutine(LoadPantsItem());
    }

    private IEnumerator LoadPantsItem()
    {
        canvasGroup.blocksRaycasts = false;
        _loadingImage.SetActive(true);
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        int condition = RuntimeData.Instance.SkinStorage.Pants.Count;
        for (int i = 0; i < condition; i++)
        {
            SkinShopItem item = _itemPool.Pull(_itemContainer);
            PantData pantData = RuntimeData.Instance.SkinStorage.Pants[i];
            item.SetItem(pantData.Sprite, i, pantData.Price, GameplayManager.Instance.UserData.UnlockedPants.Contains((byte)i), OnClickItem);
            _activeList.Add(item);
            item.transform.SetAsLastSibling();
            yield return null;
        }
        _loadingTween.Kill();
        _loadingImage.SetActive(false);
        canvasGroup.blocksRaycasts = true;
        _currentShopType = ShopType.Pants;
    }

    private void WeaponShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _selectedColor;
        ResetShop();
    }

    private void FullSetShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _selectedColor;
        ResetShop();
    }

    private void ResetShop()
    {
        for (int i = 0; i < _activeList.Count; i++)
        {
            _activeList[i].ReturnToPool();
        }
        _activeList.Clear();
    }

    private void OnClickItem(int index, SkinShopItem currentItem)
    {
        switch (_currentShopType)
        {
            case ShopType.Hair:
                CheckItemStatus(GameplayManager.Instance.UserData.UnlockedHairs, GameplayManager.Instance.UserData.EquippedHair, (byte)index);
                break;
            case ShopType.Pants:
                CheckItemStatus(GameplayManager.Instance.UserData.UnlockedPants, GameplayManager.Instance.UserData.EquippedPant, (byte)index);
                break;
        }
        _currentItemIndex = (byte)index;
        if (currentItem is not null)
            _currentItem = currentItem;
    }

    private void CheckItemStatus(List<byte> unlockItems, byte equippedItem, byte index)
    {
        if (unlockItems.Contains(index)) // Item is unlocked
        {
            _purchaseItem.gameObject.SetActive(false);
            _purchaseItemWithAds.gameObject.SetActive(false);
            _equipItem.gameObject.SetActive(true);
            if (index == equippedItem)
            {
                _equipItem.interactable = false;
            }
            else
            {
                _equipItem.interactable = true;
            }
        }
        else // Weapon is locked
        {
            _equipItem.gameObject.SetActive(false);
            _purchaseItem.gameObject.SetActive(true);
            _purchaseItemWithAds.gameObject.SetActive(true);
            if (index > 0 && unlockItems.Contains((byte)(index - 1))) // Previous item is unlocked
            {
                _purchaseItem.interactable = true;
                _purchaseItemWithAds.interactable = true;
            }
            else // Previous item is locked
            {
                _purchaseItem.interactable = false;
                _purchaseItemWithAds.interactable = false;
            }
        }
    }

    private void PurchaseItem()
    {
        if (GameplayManager.Instance.UserData.GoldAmount >= _currentItem.Price)
        {
            switch (_currentShopType)
            {
                case ShopType.Hair:
                    GameplayManager.Instance.UserData.UnlockedHairs.Add(_currentItemIndex);
                    break;
                case ShopType.Pants:
                    GameplayManager.Instance.UserData.UnlockedPants.Add(_currentItemIndex);
                    break;
            }
            GameplayManager.Instance.ChangeGoldAmount(-_currentItem.Price);
        }
        OnClickItem(_currentItemIndex, null);
        UseItem();
        _currentItem.SetUnlock();
    }

    private void PurchaseItemWithAds()
    {
        Debug.Log(_currentItemIndex);
        switch (_currentShopType)
        {
            case ShopType.Hair:
                GameplayManager.Instance.UserData.UnlockedHairs.Add(_currentItemIndex);
                break;
            case ShopType.Pants:
                GameplayManager.Instance.UserData.UnlockedPants.Add(_currentItemIndex);
                break;
        }
        OnClickItem(_currentItemIndex, null);
        UseItem();
        _currentItem.SetUnlock();
    }

    private void UseItem()
    {
        switch (_currentShopType)
        {
            case ShopType.Hair:
                GameplayManager.Instance.UserData.EquippedHair = _currentItemIndex;
                GameplayManager.Instance.Player.EquipHair(RuntimeData.Instance.SkinStorage.Hairs[_currentItemIndex].Model);
                break;
            case ShopType.Pants:
                GameplayManager.Instance.UserData.EquippedPant = _currentItemIndex;
                GameplayManager.Instance.Player.EquipPant(RuntimeData.Instance.SkinStorage.EquipPant(_currentItemIndex));
                break;
        }
        SaveManager.Instance.SaveData(GameplayManager.Instance.UserData);
        _equipItem.interactable = false;
    }
}
