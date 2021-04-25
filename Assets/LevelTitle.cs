using System;
using TMPro;
using UnityEngine;

public class LevelTitle : MonoBehaviour {
    public TextMeshProUGUI LevelNumber;

    public void SetLevelIndex(int value) {
        LevelNumber.text = (value + 1).ToString();
    }
}