using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour {
    float Period;
    public Spin Init(float period = .5f) {
        Period = period;
        return this;
    }

    // IEnumerator AnimateMove(Cell cell, MoveType moveType) {
    //     var from = transform.position;
    //     var to = cell.ToWorldPosition();
    //     float elapsed = 0;
    //     while (elapsed < Period) {
    //         elapsed += Time.deltaTime;
    //         transform.position = Vector2.Lerp(from, to, elapsed / MoveDuration);
    //         yield return null;
    //     }
    //     GetComponent<CellPosition>().SetCell(cell);
    //     IsMoving = false;
    //     OnMoved?.Invoke(moveType);
    // }
}