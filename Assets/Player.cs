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

    public Player Init(Cell cell) {
        var playerInput = GetComponent<PlayerInput>();
        Walker = GetComponent<Walker>().Init(cell);

        playerInput.enabled = false;
        StartCoroutine(Enable(playerInput));

        return this;
    }

    public void Clear() {
        Walker.Clear();
        Destroy(gameObject);
    }

    IEnumerator Enable(PlayerInput playerInput) {
        yield return new WaitForSeconds(.5f);

        playerInput.enabled = true;
    }

    void OnHorizontalMove(InputValue value) {
        if (Walker.IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;

        Walker.AttemptMove(offset, 0);
    }

    //TODO extract to Digger
    void OnVerticalMove(InputValue value) {
        if (Walker.IsMoving) return;

        var offset = (int)value.Get<float>();
        if (offset == 0) return;

        Walker.AttemptMove(0, offset);
    }
}
