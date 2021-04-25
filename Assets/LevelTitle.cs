using TMPro;
using UnityEngine;

public class LevelTitle : MonoBehaviour {
    public TextMeshProUGUI LevelNumber;
    public LevelTitle Init() {
        return this;
    }

    public void SetLevelNumber(int value) {
        LevelNumber.text = (value + 1).ToString();
    }

    public bool IsVisible { get { return gameObject.activeInHierarchy; } }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}