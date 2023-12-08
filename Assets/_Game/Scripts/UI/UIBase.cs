using UnityEngine;

public class UIBase<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] private bool _preventDisablePreviousUI;

    public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
    public bool PreventDisablePreviousUI { get => _preventDisablePreviousUI; set => _preventDisablePreviousUI = value; }
}
