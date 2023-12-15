using UnityEngine;
using DG.Tweening;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private CanvasGroup _parentCanvasGroup;
    [SerializeField] private GameObject _loadingImage;

    private CanvasGroup _currentActiveUI;
    private Tween _loadingTween;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _loadingTween = _loadingImage.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        _currentActiveUI = MenuUI.Instance.CanvasGroup;
        GameplayManager.Instance.OnGameStatePrepare += OpenMenuUI;
        GameplayManager.Instance.OnGameStatePlaying += OpenGameplayUI;
    }

    /// <summary>
    /// OnGameStatePrepare is called when the game state is preparing.
    /// </summary>
    public void OpenMenuUI()
    {
        OpenUI(MenuUI.Instance.CanvasGroup, MenuUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// OnGameStatePlaying is called when the game state is playing.
    /// </summary>
    public void OpenGameplayUI()
    {
        OpenUI(GameplayUI.Instance.CanvasGroup, GameplayUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// OnWeaponShop is called when the weapon shop is opened.
    /// </summary>
    public void OpenWeaponShopUI()
    {
        OpenUI(WeaponShopUI.Instance.CanvasGroup, WeaponShopUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// OnSkinShopEnter is called when the skin shop is opened.
    /// </summary>
    public void OpenSkinShopUI()
    {
        OpenUI(SkinShopUI.Instance.CanvasGroup, SkinShopUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// Open Revive UI.
    /// </summary>
    public void OpenReviveUI()
    {
        OpenUI(ReviveUI.Instance.CanvasGroup, ReviveUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// Open Lose UI.
    /// </summary>
    public void OpenLoseUI()
    {
        OpenUI(GameoverUI.Instance.CanvasGroup, GameoverUI.Instance.PreventDisablePreviousUI);
    }

    /// <summary>
    /// Open UI Base Function.
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="preventDisablePreviousUI"></param>
    public void OpenUI(CanvasGroup canvasGroup, bool preventDisablePreviousUI)
    {
        if (_currentActiveUI == canvasGroup) return;
        canvasGroup.gameObject.SetActive(true);
        _currentActiveUI.blocksRaycasts = false;
        _currentActiveUI.DOFade(0, _fadeDuration);
        canvasGroup.DOFade(1, _fadeDuration).OnComplete(() =>
        {
            if (!preventDisablePreviousUI)
                _currentActiveUI.gameObject.SetActive(false);
            _currentActiveUI = canvasGroup;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void AllowInteract()
    {
        _parentCanvasGroup.blocksRaycasts = true;
        _loadingTween.Kill();
        _loadingImage.SetActive(false);
    }
}
