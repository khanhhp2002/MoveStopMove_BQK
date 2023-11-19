using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : Singleton<MenuUI>
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _weaponShopOpen;
    [SerializeField] private TMP_Text _goldAmount;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        GameplayManager.Instance.OnGoldAmountChange += OnGoldAmountChange;
        NumberCounter.Instance.CountAnimation(_goldAmount, GameplayManager.Instance._userData.GoldAmount);
        _playButton.onClick.AddListener(StartGame);
        _weaponShopOpen.onClick.AddListener(OpenWeaponShop);
    }

    private void StartGame()
    {
        GameplayManager.Instance.SetGameState(GameState.Playing);
    }
    private void OpenWeaponShop()
    {
        GameplayManager.Instance.SetGameState(GameState.WeaponShopEnter);
    }

    public void OnGoldAmountChange()
    {
        NumberCounter.Instance.CountAnimation(_goldAmount, GameplayManager.Instance._userData.GoldAmount);
    }
}
