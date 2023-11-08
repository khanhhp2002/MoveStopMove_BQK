using UnityEngine;
using DG.Tweening;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CanvasGroup _menuUI;
    [SerializeField] private CanvasGroup _playingUI;
    [SerializeField] private CanvasGroup _pauseUI;
    [SerializeField] private CanvasGroup _endUI;
    [SerializeField] private float _fadeDuration = 0.5f;

    /// <summary>
    /// OnGameStatePrepare is called when the game state is preparing.
    /// </summary>
    public void OnGameStatePrepare()
    {
        _menuUI.interactable = true;
        _playingUI.interactable = false;
        _menuUI.DOFade(1, _fadeDuration);
        _playingUI.DOFade(0, _fadeDuration);
    }

    /// <summary>
    /// OnGameStatePlaying is called when the game state is playing.
    /// </summary>
    public void OnGameStatePlaying()
    {
        _menuUI.interactable = false;
        _playingUI.interactable = true;
        _menuUI.DOFade(0, _fadeDuration);
        _playingUI.DOFade(1, _fadeDuration);
    }
}
