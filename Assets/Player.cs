using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CellPosition))]
public class Player : MonoBehaviour {
    public event Action<Vector2Int> OnMovementRequired;
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public InputActionAsset ActionAsset;
    public float MoveDuration = .5f;
    bool IsMoving = false;

    public Player Init(Cell cell) {
        var playerInput = GetComponent<PlayerInput>();
        GetComponent<CellPosition>().Init(cell);

        playerInput.enabled = false;
        StartCoroutine(Enable(playerInput));

        return this;
    }

    IEnumerator Enable(PlayerInput playerInput) {
        yield return new WaitForSeconds(.5f);

        playerInput.enabled = true;
    }

    void OnHorizontalMove(InputValue value) {
        if (IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;
        Debug.Log("OnHorizontalMove " + offset);

        OnMovementRequired.Invoke(new Vector2Int(offset, 0));
    }

    void OnVerticalMove(InputValue value) {
        if (IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;

        Debug.Log("OnVerticalMove " + offset);
        OnMovementRequired.Invoke(new Vector2Int(0, offset));
    }

    public void MoveToCell(Cell cell) {
        if (IsMoving) return;

        StartCoroutine(AnimateMove(cell));
    }

    IEnumerator AnimateMove(Cell cell) {
        Debug.Log("AnimatedMove " + cell);
        IsMoving = true;
        var from = transform.position;
        var to = cell.ToVector3();
        float elapsed = 0;
        while (elapsed < MoveDuration) {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(from, to, elapsed / MoveDuration);
            yield return null;
        }
        GetComponent<CellPosition>().SetCell(cell);
        IsMoving = false;
    }
}
