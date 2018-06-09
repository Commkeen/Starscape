using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InstrumentGauge : MonoBehaviour {

    private RectTransform _rectTransform;

    private bool _isVertical;
    private float _maxLength;
    private float _targetValue;

    private bool _hasPower;

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _isVertical = _rectTransform.rect.height > _rectTransform.rect.width;
        _maxLength = _isVertical ? _rectTransform.rect.height : _rectTransform.rect.width;
        ShipInternals.Instance.RegisterInstrumentGauge(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPower(bool hasPower)
    {
        _hasPower = hasPower;
        if (hasPower)
        {
            SetValue(_targetValue);
        }
        else
        {
            SetValue(0);
        }
    }

    public void MoveToValue(float value, float speed = 0)
    {
        _targetValue = value;
        if (_hasPower)
        {
            SetValue(value);
        }
    }

    private void SetValue(float value)
    {
        float newScale = Mathf.Lerp(0, _maxLength, value);
        if (_isVertical)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newScale);
        }
        else
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newScale);
        }
    }
}
