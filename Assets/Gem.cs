using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Pickable))]
[RequireComponent(typeof(Spin))]
public class Gem : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Gem Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<Pickable>().Init(PickableType.Gem);
        GetComponent<Spin>().Init();
        return this;
    }
}