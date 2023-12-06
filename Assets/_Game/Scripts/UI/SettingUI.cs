using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : UIBase<SettingUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _returnHomeBtn;
    [SerializeField] private Button _continueBtn;
    [SerializeField] private Button _soundSetting;
    [SerializeField] private Button _vibrationSetting;

    [Header("Component Settings"), Space(5f)]
    [SerializeField] private float _xOffset;
    [SerializeField] private Sprite _toggleOn;
    [SerializeField] private Sprite _toggleOff;
    [SerializeField] private float _duration;

    private bool _isSoundOn = false;
    private bool _isVibrationOn = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        Init();
        _returnHomeBtn.onClick.AddListener(ReturnHome);
        _continueBtn.onClick.AddListener(Continue);
        _soundSetting.onClick.AddListener(SoundSetting);
        _vibrationSetting.onClick.AddListener(VibrationSetting);
    }

    /// <summary>
    /// Init the setting.
    /// </summary>
    private void Init()
    {
        _isSoundOn = GameplayManager.Instance.UserData.IsSoundEnabled;
        _isVibrationOn = GameplayManager.Instance.UserData.IsVibrationEnabled;
        _soundSetting.GetComponent<Image>().sprite = _isSoundOn ? _toggleOn : _toggleOff;
        _vibrationSetting.GetComponent<Image>().sprite = _isVibrationOn ? _toggleOn : _toggleOff;
        _soundSetting.transform.localPosition = new Vector3(_isSoundOn ? _xOffset : -_xOffset, 0, 0);
        _vibrationSetting.transform.localPosition = new Vector3(_isVibrationOn ? _xOffset : -_xOffset, 0, 0);
    }

    /// <summary>
    /// Save the setting.
    /// </summary>
    private void SaveSetting()
    {
        //Save
        GameplayManager.Instance.UserData.IsSoundEnabled = _isSoundOn;
        GameplayManager.Instance.UserData.IsVibrationEnabled = _isVibrationOn;
        SaveManager.Instance.SaveData(GameplayManager.Instance.UserData);
    }

    /// <summary>
    /// Continue the game.
    /// </summary>
    private void Continue()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenUI(GameplayUI.Instance.CanvasGroup, GameplayUI.Instance.PreventDisablePreviousUI);
        SaveSetting();
    }

    /// <summary>
    /// Return to home screen.
    /// </summary>
    private void ReturnHome()
    {
        StartCoroutine(ReturnHomeScreenAsync());
    }
    IEnumerator ReturnHomeScreenAsync()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        SaveSetting();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    /// <summary>
    /// Set the sound setting.
    /// </summary>
    private void SoundSetting()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        _isSoundOn = !_isSoundOn;
        _soundSetting.transform.DOLocalMoveX(_isSoundOn ? _xOffset : -_xOffset, _duration)
            .OnComplete(() => _soundSetting.GetComponent<Image>().sprite = _isSoundOn ? _toggleOn : _toggleOff);
    }

    /// <summary>
    /// Set the vibration setting.
    /// </summary>
    private void VibrationSetting()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        _isVibrationOn = !_isVibrationOn;
        _vibrationSetting.transform.DOLocalMoveX(_isVibrationOn ? _xOffset : -_xOffset, _duration)
            .OnComplete(() => _vibrationSetting.GetComponent<Image>().sprite = _isVibrationOn ? _toggleOn : _toggleOff);
    }
}
