using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : MonoBehaviour
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
        UIManager.Instance.OnSkinShopExit();
    }
}
