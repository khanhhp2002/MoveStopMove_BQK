using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverUI : UIBase<GameoverUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _bonusReward;
    [SerializeField] private Button _screenCapture;
    [SerializeField] private Button _returnMenu;


    [Header("Information"), Space(5f)]
    [SerializeField] private TMP_Text _ranking;
    [SerializeField] private TMP_Text _killerName;
    [SerializeField] private TMP_Text _earnedGold;

    [Header("Components"), Space(5f)]
    [SerializeField] private Slider _slider;

    [SerializeField] private Image _zone1;
    [SerializeField] private Image _zone2;
    [SerializeField] private Sprite _zoneComplete;
    [SerializeField] private Sprite _zoneIncomplete;

    [SerializeField] private Image _zone1Icon;
    [SerializeField] private Image _zone2Icon;
    [SerializeField] private Color32 _completeColor;
    [SerializeField] private Color32 _incompleteColor;

    [SerializeField] private Image _zone1CompleteStatus;
    [SerializeField] private Image _zone2CompleteStatus;
    [SerializeField] private Sprite _lock;
    [SerializeField] private Sprite _unlock;

    private bool _isPhase1Unlock = false;
    private bool _isPhase2Unlock = false;

    private void OnEnable()
    {
        Init();
        int lastZone = RuntimeData.Instance.ZoneData.CurrentZoneIndex;
        if ((GameplayManager.Instance.Player as Player).Ranking is 1)
        {
            _killerName.text = "You Win";
            _ranking.text = "#1";
            SoundManager.Instance.PlaySFX(SFXType.Win);
            if (RuntimeData.Instance.ZoneData.CurrentZoneIndex is 0)
                RuntimeData.Instance.ZoneData.CurrentZoneIndex = 1;
            else
                RuntimeData.Instance.ZoneData.CurrentZoneIndex = 0;
        }
        else
        {
            _killerName.text = ((Player)GameplayManager.Instance.Player).KillerName;
            _ranking.text = $"#{GameplayManager.Instance.AliveCounter}";
            SoundManager.Instance.PlaySFX(SFXType.Lose);
        }
        _earnedGold.text = GameplayManager.Instance.Player.KillCount.ToString();
        if (lastZone is 0)
        {
            float sliderValue = 20 + ((GameplayManager.Instance.MaxAliveCounter + 1) - (GameplayManager.Instance.Player as Player).Ranking) / (float)GameplayManager.Instance.MaxAliveCounter * 60f;
            _slider.DOValue(sliderValue, 3f).OnUpdate(() =>
            {
                canvasGroup.blocksRaycasts = false;
                if (!_isPhase1Unlock && _slider.value >= 20f)
                {
                    _isPhase1Unlock = true;
                    _zone1.sprite = _zoneComplete;
                    _zone1Icon.color = _completeColor;
                    _zone1CompleteStatus.sprite = _unlock;
                }
                else if (!_isPhase2Unlock && _slider.value >= 80f)
                {
                    _isPhase2Unlock = true;
                    _zone2.sprite = _zoneComplete;
                    _zone2Icon.color = _completeColor;
                    _zone2CompleteStatus.sprite = _unlock;
                }
            }).OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
            });
        }
        else
        {
            _isPhase1Unlock = true;
            _isPhase2Unlock = true;
            _zone1.sprite = _zoneComplete;
            _zone1Icon.color = _completeColor;
            _zone1CompleteStatus.sprite = _unlock;
            _zone2.sprite = _zoneComplete;
            _zone2Icon.color = _completeColor;
            _zone2CompleteStatus.sprite = _unlock;
            _slider.value = 80f;
            float sliderValue = 80 + ((GameplayManager.Instance.MaxAliveCounter + 1) - (GameplayManager.Instance.Player as Player).Ranking) / (float)GameplayManager.Instance.MaxAliveCounter * 20f;
            _slider.DOValue(sliderValue, 3f).OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
            });
        }
    }

    private void Init()
    {
        _isPhase1Unlock = false;
        _isPhase2Unlock = false;
        _zone1.sprite = _zoneIncomplete;
        _zone1Icon.color = _incompleteColor;
        _zone1CompleteStatus.sprite = _lock;
        _zone2.sprite = _zoneIncomplete;
        _zone2Icon.color = _incompleteColor;
        _zone2CompleteStatus.sprite = _lock;
    }

    private void Start()
    {
        _bonusReward.onClick.AddListener(BonusReward);
        _screenCapture.onClick.AddListener(ScreenCapture);
        _returnMenu.onClick.AddListener(ReturnMenu);
    }

    private void BonusReward()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        // show ad
    }

    private void ScreenCapture()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        // take screenshot
    }

    private void ReturnMenu()
    {
        StartCoroutine(ReturnHomeScreenAsync());
    }

    IEnumerator ReturnHomeScreenAsync()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        GameplayManager.Instance.UserData.GoldAmount += GameplayManager.Instance.Player.KillCount;
        SaveManager.Instance.SaveData(GameplayManager.Instance.UserData);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}
