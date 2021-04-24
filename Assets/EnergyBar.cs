using System;
using UnityEngine;

public class EnergyBar : MonoBehaviour {
    public event Action OnEmpty;
    public RectTransform Background;
    public RectTransform Gauge;
    int Value;
    int MaxValue;

    public EnergyBar Init(int maxValue) {
        Value = MaxValue = maxValue;
        return this;
    }

    public void Increment(int value = 1) {
        SetValue(Value + value);
        if (Value == 0) OnEmpty?.Invoke();
    }

    public void Decrement(int value = 1) {
        Debug.Log("decrement");
        SetValue(Value - value);
    }

    void SetValue(int value) {
        if (value < 0) return;

        if (value > MaxValue) value = MaxValue;

        Value = value;

        var top = -(float)(MaxValue - Value) / MaxValue * Background.rect.height;
        Gauge.offsetMax = new Vector2(0, top);
    }
}