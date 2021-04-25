using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Picker))]
[RequireComponent(typeof(Walker))]
public class Player : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Walker Walker { get; private set; }
    InputAction.CallbackContext CallbackContext;

    public Player Init(Cell cell) {
        var playerInput = GetComponent<PlayerInput>();
        Walker = GetComponent<Walker>().Init(cell);

        playerInput.enabled = false;
        StartCoroutine(Enable(playerInput));

        playerInput.onActionTriggered += OnActionTriggered;

        return this;
    }

    void OnActionTriggered(InputAction.CallbackContext context) {
        Debug.Log(context.phase);
        CallbackContext = context;
    }

    public void Clear() {
        Walker.Clear();
        Destroy(gameObject);
    }

    IEnumerator Enable(PlayerInput playerInput) {
        yield return new WaitForSeconds(.5f);

        playerInput.enabled = true;
    }

    void Update() {
        switch (CallbackContext.action?.name) {
            case "HorizontalMove": HandleHorizontalMove(); break;
            case "VerticalMove": HandleVerticalMove(); break;
        }
    }

    void HandleHorizontalMove() {
        switch (CallbackContext.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                OnHorizontalMove(CallbackContext.ReadValue<float>());
                break;
        }
    }

    void HandleVerticalMove() {
        switch (CallbackContext.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                OnVerticalMove(CallbackContext.ReadValue<float>());
                break;
        }
    }

    void OnHorizontalMove(float value) {
        if (Walker.IsMoving) return;

        var offset = (int)value;
        if (offset == 0) return;

        Walker.AttemptMove(offset, 0);
    }

    //TODO extract to Digger
    void OnVerticalMove(float value) {
        if (Walker.IsMoving) return;

        var offset = (int)value;
        if (offset == 0) return;

        Walker.AttemptMove(0, offset);
    }
}
