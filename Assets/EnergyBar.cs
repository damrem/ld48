using System;
using Damrem.Collections;
using Damrem.UnityEngine;
using UnityEngine;
// using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {
    event Action OnEmpty;
    public EnergyPoint EnergyPointPrefab;
    EnergyPoint[] Points;
    int Value;
    int MaxValue;

    public EnergyBar Init(int maxValue, Action onEmpty) {
        Clear();

        Value = MaxValue = maxValue;
        OnEmpty += onEmpty;
        Points = new EnergyPoint[maxValue];
        Points.Fill(CreatePoint);
        return this;
    }

    void Clear() {
        gameObject.DestroyChildren();
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
        Debug.Log(Value);
        Debug.Log(Value <= 0);
        if (Value <= 0) OnEmpty?.Invoke();
    }

    void SetValue(int value) {
        if (value < 0) value = 0;
        else if (value > MaxValue) value = MaxValue;

        Value = value;
        UpdatePoints();
    }
}