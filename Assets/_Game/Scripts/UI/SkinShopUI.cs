using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Button _useWeapon;

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
        _hairShopBtn.onClick.AddListener(HairShop);
        _pantsShopBtn.onClick.AddListener(PantsShop);
        _weaponShopBtn.onClick.AddListener(WeaponShop);
        _fullSetShopBtn.onClick.AddListener(FullSetShop);
        _hairShopBtn.Select();
        HairShop();
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
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
    }

    private IEnumerator LoadHairItem()
    {
        canvasGroup.interactable = false;
        _loadingImage.SetActive(true);
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        int condition = RuntimeData.Instance.SkinStorage.Hairs.Count;
        for (int i = 0; i < condition; i++)
        {
            SkinShopItem item = _itemPool.Pull(_itemContainer);
            item.SetItem(RuntimeData.Instance.SkinStorage.Hairs[i].Sprite, i, GameplayManager.Instance.UserData.UnlockedHairs.Contains((byte)i));
            _activeList.Add(item);
            item.transform.SetAsLastSibling();
            yield return null;
        }
        _loadingTween.Kill();
        _loadingImage.SetActive(false);
        canvasGroup.interactable = true;
    }

    private void PantsShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _selectedColor;
        ResetShop();
        StartCoroutine(LoadPantsItem());
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
    }

    private IEnumerator LoadPantsItem()
    {
        canvasGroup.interactable = false;
        _loadingImage.SetActive(true);
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        int condition = RuntimeData.Instance.SkinStorage.Pants.Count;
        for (int i = 0; i < condition; i++)
        {
            SkinShopItem item = _itemPool.Pull(_itemContainer);
            item.SetItem(RuntimeData.Instance.SkinStorage.Pants[i].Sprite, i, GameplayManager.Instance.UserData.UnlockedPants.Contains((byte)i));
            _activeList.Add(item);
            item.transform.SetAsLastSibling();
            yield return null;
        }
        _loadingTween.Kill();
        _loadingImage.SetActive(false);
        canvasGroup.interactable = true;
    }

    private void WeaponShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _selectedColor;
        ResetShop();
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
    }

    private void FullSetShop()
    {
        _hairShopBtn.image.color = _unselectedColor;
        _pantsShopBtn.image.color = _unselectedColor;
        _weaponShopBtn.image.color = _unselectedColor;
        _fullSetShopBtn.image.color = _selectedColor;
        ResetShop();
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
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

    private void ResetShop()
    {
        for (int i = 0; i < _activeList.Count; i++)
        {
            _activeList[i].ReturnToPool();
        }
        _activeList.Clear();
    }
}
