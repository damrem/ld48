using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour {
    InputAction.CallbackContext CallbackContext;
    public Direction InputDirection { get; private set; } = Direction.None;

    public PlayerControl Init() {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
        playerInput.enabled = false;
        StartCoroutine(Enable(playerInput));
        return this;
    }

    void OnActionTriggered(InputAction.CallbackContext context) {
        CallbackContext = context;
    }

    IEnumerator Enable(PlayerInput playerInput) {
        yield return new WaitForSeconds(.5f);
        playerInput.enabled = true;
    }

    void Update() {
        switch (CallbackContext.action?.name) {
            case "HorizontalMove": HandleHorizontalMove(); break;
            case "VerticalMove": HandleVerticalMove(); break;
            default: InputDirection = Direction.None; break;
        }
    }

    void HandleHorizontalMove() {
        switch (CallbackContext.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                OnHorizontalMove(CallbackContext.ReadValue<float>());
                break;

            default:
            case InputActionPhase.Canceled:
                InputDirection = Direction.None;
                break;
        }
    }

    void HandleVerticalMove() {
        switch (CallbackContext.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                OnVerticalMove(CallbackContext.ReadValue<float>());
                break;

            default:
            case InputActionPhase.Canceled:
                InputDirection = Direction.None;
                break;
        }
    }

    void OnHorizontalMove(float value) {
        var offset = (int)value;
        if (offset == 0) return;

        InputDirection = offset == 1 ? Direction.Right : Direction.Left;
    }

    void OnVerticalMove(float value) {
        var offset = (int)value;
        if (offset == 0) return;

        InputDirection = offset == 1 ? Direction.Down : Direction.Up;
    }
}
