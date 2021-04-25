using TMPro;
using UnityEngine;

public class Purse : MonoBehaviour {
    public int Value { get; private set; } = 0;
    TextMeshProUGUI TextMesh;

    public Purse Init(int initialValue = 0) {
        TextMesh = GetComponentInChildren<TextMeshProUGUI>();
        SetValue(initialValue);
        return this;
    }

    public void Increment(int value = 1) {
        SetValue(Value + value);
    }

    void SetValue(int value) {
        Value = value;
        TextMesh.text = value.ToString();
    }
}