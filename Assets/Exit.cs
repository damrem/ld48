using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Collider2D))]
public class Exit : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Exit Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        return this;
    }
}