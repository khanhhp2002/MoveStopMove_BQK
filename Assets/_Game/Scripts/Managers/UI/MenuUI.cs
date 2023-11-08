using UnityEngine;
using UnityEngine.UI;

public class MenuUI : Singleton<MenuUI>
{
    [SerializeField] private Button _playButton;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _playButton.onClick.AddListener(() => GameplayManager.Instance.SetGameState(GameState.Playing));
    }
}
