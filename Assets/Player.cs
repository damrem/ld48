using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Picker))]
public class Player : MonoBehaviour {
    public event Action<Vector2Int> OnMovementRequired;
    public event Action<MoveType> OnMoved;
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public float MoveDuration = .5f;
    bool IsMoving = false;

    public Player Init(Cell cell) {
        var playerInput = GetComponent<PlayerInput>();
        GetComponent<CellPosition>().Init(cell);

        playerInput.enabled = false;
        StartCoroutine(Enable(playerInput));

        return this;
    }

    public void Clear() {
        OnMovementRequired = default;
        OnMoved = default;
        Destroy(gameObject);
    }

    IEnumerator Enable(PlayerInput playerInput) {
        yield return new WaitForSeconds(.5f);

        playerInput.enabled = true;
    }

    void OnHorizontalMove(InputValue value) {
        if (IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;

        OnMovementRequired.Invoke(new Vector2Int(offset, 0));
    }

    void OnVerticalMove(InputValue value) {
        if (IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;

        OnMovementRequired.Invoke(new Vector2Int(0, offset));
    }

    public void MoveToCell(Cell cell) {
        if (IsMoving) return;

        var moveType = cell.X != Cell.X ? MoveType.Walk : MoveType.Fall;

        StartCoroutine(AnimateMove(cell, moveType));
    }

    IEnumerator AnimateMove(Cell cell, MoveType moveType) {
        IsMoving = true;
        var from = transform.position;
        var to = cell.ToWorldPosition();
        float elapsed = 0;
        while (elapsed < MoveDuration) {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(from, to, elapsed / MoveDuration);
            yield return null;
        }
        GetComponent<CellPosition>().SetCell(cell);
        IsMoving = false;
        OnMoved?.Invoke(moveType);
    }
}
