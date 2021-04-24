using UnityEngine;

public class CellPosition : MonoBehaviour {
    public Cell Cell { get; private set; }
    public CellPosition Init(Cell cell) {
        SetCell(cell);
        return this;
    }

    public void SetCell(Cell cell) {
        Cell = cell;
        transform.position = new Vector2(cell.X, -cell.Y);
    }
}