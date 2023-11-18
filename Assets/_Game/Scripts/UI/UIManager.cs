using UnityEngine;
using DG.Tweening;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CanvasGroup _menuUI;
    [SerializeField] private CanvasGroup _playingUI;
    [SerializeField] private CanvasGroup _pauseUI;
    [SerializeField] private CanvasGroup _endUI;
    [SerializeField] private CanvasGroup _weaponShopUI;
    [SerializeField] private float _fadeDuration = 0.5f;

    /// <summary>
    /// OnGameStatePrepare is called when the game state is preparing.
    /// </summary>
    public void OnGameStatePrepare()
    {
        _menuUI.interactable = true;
        _menuUI.gameObject.SetActive(true);
        _playingUI.interactable = false;
        _menuUI.DOFade(1, _fadeDuration);
        _playingUI.DOFade(0, _fadeDuration);
        _playingUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// OnGameStatePlaying is called when the game state is playing.
    /// </summary>
    public void OnGameStatePlaying()
    {
        _menuUI.interactable = false;
        _playingUI.interactable = true;
        _playingUI.gameObject.SetActive(true);
        _menuUI.DOFade(0, _fadeDuration);
        _playingUI.DOFade(1, _fadeDuration);
        _menuUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// OnWeaponShop is called when the weapon shop is opened.
    /// </summary>
    public void OnWeaponShopEnter()
    {
        _menuUI.interactable = false;
        _weaponShopUI.interactable = true;
        _weaponShopUI.gameObject.SetActive(true);
        _menuUI.DOFade(0, _fadeDuration);
        _weaponShopUI.DOFade(1, _fadeDuration);
    }

    public void OnWeaponShopExit()
    {
        _menuUI.interactable = true;
        _weaponShopUI.interactable = false;
        _menuUI.DOFade(1, _fadeDuration);
        _weaponShopUI.DOFade(0, _fadeDuration);
        _weaponShopUI.gameObject.SetActive(false);
    }
}
