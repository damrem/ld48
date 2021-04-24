using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Pickable))]
[RequireComponent(typeof(Spin))]
public class Coin : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Coin Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<Pickable>().Init(PickableType.Coin);
        GetComponent<Spin>().Init();
        return this;
    }
}