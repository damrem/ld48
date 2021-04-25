using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Overlay))]
public class LevelTitle : MonoBehaviour {
    public TextMeshProUGUI LevelNumber;
    public Overlay Overlay { get { return GetComponent<Overlay>(); } }

    public LevelTitle Init(Action onInteracted) {
        Overlay.Init(onInteracted);
        return this;
    }
    public void SetLevelNumber(int value) {
        LevelNumber.text = (value + 1).ToString();
    }
}