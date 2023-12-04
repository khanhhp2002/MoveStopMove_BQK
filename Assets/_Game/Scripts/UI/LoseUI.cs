using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUI : UIBase<LoseUI>
{
    [Header("Action Buttons"), Space(5f)]
    [SerializeField] private Button _bonusReward;
    [SerializeField] private Button _screenCapture;
    [SerializeField] private Button _returnMenu;


    [Header("Information"), Space(5f)]
    [SerializeField] private TMP_Text _ranking;
    [SerializeField] private TMP_Text _killerName;
    [SerializeField] private TMP_Text _earnedGold;

    private void OnEnable()
    {
        /*_ranking.text = GameplayManager.Instance.UserData.Ranking.ToString();
        _killerName.text = GameplayManager.Instance.UserData.KillerName;
        _earnedGold.text = GameplayManager.Instance.UserData.EarnedGold.ToString();*/
    }

    private void Start()
    {
        _bonusReward.onClick.AddListener(BonusReward);
        _screenCapture.onClick.AddListener(ScreenCapture);
        _returnMenu.onClick.AddListener(ReturnMenu);
    }

    private void BonusReward()
    {
        // show ad
    }

    private void ScreenCapture()
    {
        // take screenshot
    }

    private void ReturnMenu()
    {
        // reload scene
        //GameplayManager.Instance.SetGameState(GameState.Preparing);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
