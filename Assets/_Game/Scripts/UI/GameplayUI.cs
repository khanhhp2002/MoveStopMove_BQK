using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBase<GameplayUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _settingButton;

    [SerializeField] TMP_Text _aliveCounterDisplay;

    private void Start()
    {
        GameplayManager.Instance.OnCounterChange += OnCounterChange;
        _settingButton.onClick.AddListener(OpenSetting);
    }

    /// <summary>
    /// On Counter Change call when the alive counter change.
    /// </summary>
    private void OnCounterChange()
    {
        _aliveCounterDisplay.text = $"Alive: {GameplayManager.Instance.AliveCounter}";
    }

    private void OpenSetting()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenUI(SettingUI.Instance.CanvasGroup, SettingUI.Instance.PreventDisablePreviousUI);
    }
}
