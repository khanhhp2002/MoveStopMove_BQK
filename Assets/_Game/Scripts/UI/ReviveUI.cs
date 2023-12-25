using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveUI : UIBase<ReviveUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _reviveWithGold;
    [SerializeField] private Button _reviveWithAds;
    [SerializeField] private Button _skip;

    [Header("Loading Components"), Space(5f)]
    [SerializeField] private GameObject _loadingImage;
    [SerializeField] private TMP_Text _timeCounter;
    [SerializeField] private int _maxTime = 5;

    private Tween _loadingTween;
    private void OnEnable()
    {
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        StartCoroutine(TimeCounter());
    }

    private void Start()
    {
        _reviveWithGold.onClick.AddListener(ReviveWithGold);
        _reviveWithAds.onClick.AddListener(ReviveWithAds);
        _skip.onClick.AddListener(Skip);
    }

    private void ReviveWithGold()
    {
        OnClose();
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        this.gameObject.SetActive(false);
        ((Player)GameplayManager.Instance.Player).Revive();
        UIManager.Instance.OpenGameplayUI();
        // respawn player
    }

    private void ReviveWithAds()
    {
        OnClose();
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        this.gameObject.SetActive(false);
        ((Player)GameplayManager.Instance.Player).Revive();
        UIManager.Instance.OpenGameplayUI();
        // respawn player
    }

    private void Skip()
    {
        OnClose();
        this.gameObject.SetActive(false);
        GameplayManager.Instance.SetGameState(GameState.GameOver);
    }

    private IEnumerator TimeCounter()
    {
        int time = _maxTime;
        while (time >= 0)
        {
            _timeCounter.text = time.ToString();
            SoundManager.Instance.PlaySFX(SFXType.CountDown);
            yield return new WaitForSeconds(1f);
            time--;
        }
        Skip();
    }

    private void OnClose()
    {
        _loadingTween.Kill();
        StopAllCoroutines();
    }

}
