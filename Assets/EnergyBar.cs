using System;
using Damrem.Collections;
using UnityEngine;
// using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {
    public event Action OnEmpty;
    // public RectTransform Background;
    // public RectTransform Gauge;
    public EnergyPoint EnergyPointPrefab;
    EnergyPoint[] Points;
    int Value;
    int MaxValue;

    public EnergyBar Init(int maxValue) {
        Value = MaxValue = maxValue;
        Points = new EnergyPoint[maxValue];
        Points.Fill(CreatePoint);
        return this;
    }

    EnergyPoint CreatePoint() {
        return Instantiate(EnergyPointPrefab, transform).Init();
    }

    public void Increment(int value = 1) {
        SetValue(Value + value);
    }

    void UpdatePoints() {
        Points.ToList().ForEach((point, i) => {
            if (i < Value) point.Fill();
            else point.Empty();
        });
    }

    public void Decrement(int value = 1) {
        SetValue(Value - value);
        if (Value == 0) OnEmpty?.Invoke();
    }

    void SetValue(int value) {
        if (value < 0) return;

        if (value > MaxValue) value = MaxValue;

        Value = value;

        // var top = -(float)(MaxValue - Value) / MaxValue * (Background.rect.height - 4);
        // if (top > -2) top = -2;
        // Gauge.offsetMax = new Vector2(-2, top);

        UpdatePoints();
    }
}