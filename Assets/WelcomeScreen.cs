using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WelcomeScreen : MonoBehaviour, IPointerClickHandler {
    public event Action OnClicked;

    public void OnPointerClick(PointerEventData data) {
        OnClicked.Invoke();
    }

    void Update() {
        if (Keyboard.current.anyKey.wasReleasedThisFrame) {
            Debug.Log("Update");
            OnClicked.Invoke();
        }
    }
}