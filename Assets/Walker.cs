using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
public class Walker : MonoBehaviour {
    public event Action<MoveType> OnMoved;
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public float MoveDuration = .5f;
    public bool IsMoving { get; private set; } = false;
    public Transform Head;
    public Transform Body;
    Coroutine WalkCoroutine;
    Vector3 HeadIdlePosition;

    public Walker Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        HeadIdlePosition = Head.localPosition;
        return this;
    }

    public void Clear() {
        OnMoved = default;
    }

    public void MoveToCell(Cell cell, Action onEnd = default) {
        if (IsMoving) return;

        var moveType = cell.X != Cell.X ? MoveType.Walk : MoveType.Fall;

        StartCoroutine(AnimateMove(cell, moveType, () => {
            onEnd?.Invoke();
            StopCoroutine(WalkCoroutine);
            IdleHead();
        }));
        WalkCoroutine = StartCoroutine(AnimateWalk());
    }

    IEnumerator AnimateWalk() {
        float elapsed = 0;

        while (IsMoving) {
            elapsed += Time.deltaTime;
            Head.localPosition = HeadIdlePosition + Vector3.down * Mathf.Cos(elapsed * 20) / 8;
            // transform.localScale = new Vector3(1, .75f + .25f * Mathf.Cos(elapsed * 20), 1);
            yield return null;
        }
        IdleHead();
    }

    void IdleHead() {
        Head.localPosition = HeadIdlePosition;
    }

    IEnumerator AnimateMove(Cell cell, MoveType moveType, Action onEnd = default) {
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
        onEnd?.Invoke();
    }
}