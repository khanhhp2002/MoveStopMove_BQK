using System.Collections;
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
        SoundManager.Instance.PlaySFX(SFXType.Lose);
        _earnedGold.text = GameplayManager.Instance.Player.KillCount.ToString();
        _killerName.text = ((Player)GameplayManager.Instance.Player).KillerName;
        _ranking.text = $"#{GameplayManager.Instance.AliveCounter}";
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
