using UnityEngine;

public class JoyStick : Singleton<JoyStick>
{
    [Header("JoyStick Components"), Space(5f)]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _range;
    [SerializeField] private GameObject _center;

    private int _maxRange;
    private bool _fixJoyStickPosition;
    private bool _hasNewPosition = false;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        transform.localScale = new Vector3(1, 1, 1);
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _fixJoyStickPosition = false;
        _maxRange = (int)(_rectTransform.rect.width * transform.localScale.x / 2);
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !_fixJoyStickPosition)
        {
            _hasNewPosition = true;
            _range.transform.position = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!_hasNewPosition && !_fixJoyStickPosition)
            {
                _range.transform.position = Input.mousePosition;
                _hasNewPosition = true;
            }
            Vector3 mousePos = Input.mousePosition;
            Vector3 offset = mousePos - _range.transform.position;
            if (offset.sqrMagnitude > Mathf.Pow(_maxRange, 2))
            {
                _center.transform.localPosition = offset.normalized * _maxRange / transform.localScale.x;
            }
            else
            {
                _center.transform.position = mousePos;
            }
        }
        else
        {
            _hasNewPosition = false;
            _center.transform.localPosition = Vector3.zero;
        }
    }

    public Vector3 GetDirection()
    {
        return _center.transform.localPosition.normalized;
    }
}
