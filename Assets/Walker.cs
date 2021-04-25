using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
public class Walker : MonoBehaviour {
    public event Action<MoveType, Cell> OnMoved;
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
            if (WalkCoroutine != null) StopCoroutine(WalkCoroutine);
        }));

        if (moveType == MoveType.Walk)
            WalkCoroutine = StartCoroutine(AnimateWalk());
    }

    void AnimateFall() {
        Debug.Log("AnimateFall " + Time.time);
        Head.localPosition = HeadIdlePosition + Vector3.up / 4;
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
        Debug.Log("IdleHead " + Time.time);
        Head.localPosition = HeadIdlePosition;
    }

    IEnumerator AnimateMove(Cell cell, MoveType moveType, Action onEnd = default) {
        Debug.Log("AnimateMove " + moveType);
        if (moveType == MoveType.Fall) AnimateFall();
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
        OnMoved?.Invoke(moveType, cell);
        onEnd?.Invoke();
        if (moveType == MoveType.Fall) IdleHead();
    }
}