using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Collider2D))]
public class Exit : MonoBehaviour {
    public Exit Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        return this;
    }
}