using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
public class Walker : MonoBehaviour {
    public event Action<Vector2Int> OnMovementRequired;
    public event Action<MoveType> OnMoved;
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public float MoveDuration = .5f;
    public bool IsMoving { get; private set; } = false;

    public Walker Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        return this;
    }

    public void Clear() {
        OnMovementRequired = default;
        OnMoved = default;
    }

    public void AttemptMove(int x, int y) {
        OnMovementRequired.Invoke(new Vector2Int(x, y));
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