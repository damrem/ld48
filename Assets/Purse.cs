using TMPro;
using UnityEngine;

public class Purse : MonoBehaviour {
    int Value = 0;
    TextMeshProUGUI TextMesh;

    void Awake() {
        TextMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public Purse Init() {
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