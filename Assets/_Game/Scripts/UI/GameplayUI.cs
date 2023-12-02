using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] TMP_Text _aliveCounterDisplay;

    private void Start()
    {
        GameplayManager.Instance.OnCounterChange += OnCounterChange;
    }

    /// <summary>
    /// On Counter Change call when the alive counter change.
    /// </summary>
    private void OnCounterChange()
    {
        _aliveCounterDisplay.text = $"Alive: {GameplayManager.Instance.AliveCounter}";
    }
}
