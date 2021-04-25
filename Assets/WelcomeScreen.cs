using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WelcomeScreen : MonoBehaviour, IPointerClickHandler {
    public event Action OnClicked;
    public void OnPointerClick(PointerEventData data) {
        OnClicked.Invoke();
    }
}