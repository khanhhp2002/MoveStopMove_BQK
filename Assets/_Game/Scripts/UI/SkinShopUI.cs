using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : UIBase<SkinShopUI>
{
    [Header("Button"), Space(5f)]
    [SerializeField] private Button _exitShop;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _exitShop.onClick.AddListener(ExitShop);
    }

    /// <summary>
    /// Exit shop.
    /// </summary>
    private void ExitShop()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        UIManager.Instance.OpenMenuUI();
    }
}
