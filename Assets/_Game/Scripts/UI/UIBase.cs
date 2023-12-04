using UnityEngine;

public class UIBase<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _preventDisablePreviousUI;

    public CanvasGroup CanvasGroup { get => _canvasGroup; set => _canvasGroup = value; }
    public bool PreventDisablePreviousUI { get => _preventDisablePreviousUI; set => _preventDisablePreviousUI = value; }
}
