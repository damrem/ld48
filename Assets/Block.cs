using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(MeshRenderer))]
public class Block : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public bool IsUnbreakable { get; private set; } = false;
    public Block Init(Cell cell, Color color) {
        GetComponent<CellPosition>().Init(cell);
        SetColor(color);
        return this;
    }

    public void SetColor(Color color) {
        GetComponent<MeshRenderer>().material.color = color;
    }

    public void SetUnbreakable() {
        IsUnbreakable = true;
        SetColor(Color.gray);
    }
}