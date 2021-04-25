using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Pickable))]
public class Gem : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Gem Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<Pickable>().Init(PickableType.Gem);
        GetComponentInChildren<ItemFloat>().Init();
        return this;
    }
}