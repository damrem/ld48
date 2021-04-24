using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(MeshRenderer))]
public class Block : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Block Init(Cell cell, Color color) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<MeshRenderer>().material.color = color;
        return this;
    }
}