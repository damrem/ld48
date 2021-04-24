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
        Debug.Log("OnHorizontalMove " + value.Get<float>());
        OnMovementRequired.Invoke(new Vector2Int((int)value.Get<float>(), 0));
    }

    void OnVerticalMove(InputValue value) {
        if (IsMoving) return;
        Debug.Log("OnVerticalMove " + value.Get<float>());
        OnMovementRequired.Invoke(new Vector2Int(0, (int)value.Get<float>()));
    }

    public void MoveToCell(Cell cell) {
        if (IsMoving) return;

        StartCoroutine(AnimateMove(cell));
    }

    IEnumerator AnimateMove(Cell cell) {
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
