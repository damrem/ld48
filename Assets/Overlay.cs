using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Overlay : MonoBehaviour, IPointerClickHandler {
    event Action OnClicked;

    public Overlay Init(Action onInteracted) {
        OnClicked += onInteracted;
        OnClicked += Hide;
        return this;
    }

    public void OnPointerClick(PointerEventData data) {
        OnClicked.Invoke();
    }

    void Update() {
        if (Keyboard.current.anyKey.wasReleasedThisFrame) {
            Debug.Log("Update");
            OnClicked?.Invoke();
        }
    }

    public bool IsVisible { get { return gameObject.activeInHierarchy; } }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}