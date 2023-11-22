using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickColor : MonoBehaviour
{
    [SerializeField] private Slider _R;
    [SerializeField] private Slider _G;
    [SerializeField] private Slider _B;

    [SerializeField] private Image _image;

    private void Start()
    {
        _R.onValueChanged.AddListener(OnValueChanged);
        _G.onValueChanged.AddListener(OnValueChanged);
        _B.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _image.color = new Color32((byte)_R.value, (byte)_G.value, (byte)_B.value, 255);
    }
}
