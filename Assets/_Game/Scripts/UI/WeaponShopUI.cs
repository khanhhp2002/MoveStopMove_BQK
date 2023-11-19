using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : Singleton<WeaponShopUI>
{
    [Header("Button"), Space(5f)]
    [SerializeField] private Button _exitShop;
    [SerializeField] private Button _nextItem;
    [SerializeField] private Button _previousItem;
    [SerializeField] private Button _purchaseItem;

    [Header("Weapon-Display Settings"), Space(5f)]
    [SerializeField] private float spawnDistanceX = 1f;
    [SerializeField] private TMP_Text _weaponName;
    [SerializeField] private TMP_Text _weaponUnlockCondition;
    [SerializeField] private TMP_Text _weaponBonusStats;
    [SerializeField] private TMP_Text _weaponPrice;

    private int _currentWeaponIndex = 0;
    private GameObject _currentWeapon;
    private WeaponData _currentWeaponData;
    private Quaternion _spawnQuaternion;

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (_spawnQuaternion == null)
            _spawnQuaternion = Camera.main.transform.rotation;
        _currentWeaponIndex = 0;
        ShowWeapon();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _exitShop.onClick.AddListener(ExitShop);
        _nextItem.onClick.AddListener(NextItem);
        _previousItem.onClick.AddListener(PreviousItem);
        _purchaseItem.onClick.AddListener(PurchaseItem);
    }

    /// <summary>
    /// Exit shop.
    /// </summary>
    private void ExitShop()
    {
        GameplayManager.Instance.SetGameState(GameState.WeaponShopExit);
        Destroy(_currentWeapon);
        _currentWeapon = null;
    }

    /// <summary>
    /// Go to next item.
    /// </summary>
    private void NextItem()
    {
        _currentWeaponIndex++;
        if (_currentWeaponIndex >= WeaponManager.Instance.GetWeaponDataCount())
        {
            _currentWeaponIndex = 0;
        }
        ShowWeapon();
    }

    /// <summary>
    /// Go to previous item.
    /// </summary>
    private void PreviousItem()
    {
        _currentWeaponIndex--;
        if (_currentWeaponIndex < 0)
        {
            _currentWeaponIndex = WeaponManager.Instance.GetWeaponDataCount() - 1;
        }
        ShowWeapon();
    }

    /// <summary>
    /// Show weapon on screen.
    /// </summary>
    private void ShowWeapon()
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon);
        }

        _currentWeaponData = WeaponManager.Instance.GetWeaponDataByIndex(_currentWeaponIndex);
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistanceX;
        _currentWeapon = Instantiate(_currentWeaponData.WeaponModel, spawnPosition, Quaternion.Euler(new Vector3(_spawnQuaternion.eulerAngles.x, _spawnQuaternion.eulerAngles.y, _currentWeaponData.ScreenWeaponZAngle)));
        _currentWeapon.transform.localScale = _currentWeaponData.ScreenWeaponScale * Vector3.one;
        _currentWeapon.transform.position += _currentWeapon.transform.right * _currentWeaponData.ScreenWeaponOffsetX;
        _currentWeapon.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        _weaponName.text = _currentWeaponData.Name;

        _weaponUnlockCondition.text = $"( {_currentWeaponData.PurchaseRequirement} )";

        _weaponBonusStats.text = string.Empty;

        if (_currentWeaponData.BonusAttackRange > 0)
        {
            _weaponBonusStats.text += $"+ {_currentWeaponData.BonusAttackRange} Range\n";
        }
        if (_currentWeaponData.BonusAttackSpeed > 0)
        {
            _weaponBonusStats.text += $"+ {_currentWeaponData.BonusAttackSpeed} Speed";
        }
        if (string.IsNullOrEmpty(_weaponBonusStats.text))
        {
            _weaponBonusStats.text = "No bonus stats";
        }
        _weaponPrice.text = _currentWeaponData.PurchasePrice.ToString();
    }

    /// <summary>
    /// Purchase item.
    /// </summary>
    private void PurchaseItem()
    {
        if (GameplayManager.Instance._userData.GoldAmount >= WeaponManager.Instance.GetWeaponDataByIndex(_currentWeaponIndex).PurchasePrice)
        {
            GameplayManager.Instance.ChangeGoldAmount(-WeaponManager.Instance.GetWeaponDataByIndex(_currentWeaponIndex).PurchasePrice);
            GameplayManager.Instance._userData.UnlockedWeapons.Add(_currentWeaponIndex);
            GameplayManager.Instance.Player.EquipWeapon(_currentWeaponData);
            GameplayManager.Instance._userData.EquippedWeapon = _currentWeaponIndex;
            //SaveManager.Instance.SaveData(GameplayManager.Instance._userData);
            ShowWeapon();
        }
    }

}