using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIBase<MenuUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _weaponShopOpen;
    [SerializeField] private Button _skinShopOpen;
    [Header("Vibration Settings"), Space(5f)]
    [SerializeField] private Button _vibrationButton;
    [SerializeField] private Image _vibrationImage;
    [SerializeField] private Sprite _vibrationOn;
    [SerializeField] private Sprite _vibrationOff;
    [Header("Vibration Settings"), Space(5f)]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [Header("Gold Settings"), Space(5f)]
    [SerializeField] private TMP_Text _goldAmount;

    private bool _isVibrationOn = true;
    private bool _isSoundOn = true;
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        GameplayManager.Instance.OnGoldAmountChange += OnGoldAmountChange;
        NumberCounter.Instance.CountAnimation(_goldAmount, GameplayManager.Instance.UserData.GoldAmount);
        _playButton.onClick.AddListener(StartGame);
        _weaponShopOpen.onClick.AddListener(OpenWeaponShop);
        _skinShopOpen.onClick.AddListener(OpenSkinShop);
        _vibrationButton.onClick.AddListener(VibrationSetting);
        _soundButton.onClick.AddListener(SoundSetting);
    }

    /// <summary>
    /// Play the game.
    /// </summary>
    private void StartGame()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        GameplayManager.Instance.SetGameState(GameState.Playing);
    }
    /// <summary>
    /// Open weapon shop.
    /// </summary>
    private void OpenWeaponShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenWeaponShopUI();
    }

    /// <summary>
    /// Open skin shop.
    /// </summary>
    private void OpenSkinShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenSkinShopUI();
    }

    /// <summary>
    /// Change gold amount in UI.
    /// </summary>
    private void OnGoldAmountChange()
    {
        NumberCounter.Instance.CountAnimation(_goldAmount, GameplayManager.Instance.UserData.GoldAmount);
    }

    /// <summary>
    /// Change vibration setting.
    /// </summary>
    private void VibrationSetting()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        _vibrationImage.sprite = _isVibrationOn ? _vibrationOff : _vibrationOn;
        _isVibrationOn = !_isVibrationOn;
    }

    private void SoundSetting()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        _soundImage.sprite = _isSoundOn ? _soundOff : _soundOn;
        _isSoundOn = !_isSoundOn;
    }
}
