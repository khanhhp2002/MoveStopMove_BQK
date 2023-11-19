using System.Collections;
using TMPro;
using UnityEngine;

public class NumberCounter : Singleton<NumberCounter>
{
    [SerializeField] private float _duration;
    [SerializeField] private float _countFps;
    [SerializeField] private bool _useGameFpsInstead;
    private Coroutine _countingCorountine;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (_useGameFpsInstead)
            _countFps = 1 / Time.fixedDeltaTime;
    }

    /// <summary>
    /// Counting animation.
    /// </summary>
    /// <param name="displayText"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    private IEnumerator CountText(TMP_Text displayText, int newValue)
    {
        int oldValue = int.Parse(displayText.text);
        if (_useGameFpsInstead)
            _countFps = 1 / Time.fixedDeltaTime;
        WaitForSeconds wait = new WaitForSeconds(1 / _countFps);
        float previousValue = oldValue;
        float stepAmount;
        if (newValue - previousValue < 0)
        {
            stepAmount = ((newValue - previousValue) / (_countFps * _duration));
        }
        else
        {
            stepAmount = ((newValue - previousValue) / (_countFps * _duration));
        }
        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue = (stepAmount + previousValue);
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }
                displayText.text = Mathf.FloorToInt(previousValue).ToString();
                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue = (stepAmount + previousValue);
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }
                displayText.text = Mathf.CeilToInt(previousValue).ToString();
                yield return wait;
            }
        }
    }

    /// <summary>
    /// Counting animation.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="suffix"></param>
    public void CountAnimation(TMP_Text input, int to)
    {
        if (_countingCorountine != null)
        {
            StopCoroutine(_countingCorountine);
        }
        StartCoroutine(CountText(input, to));
    }
}
