using Damrem.UnityEngine;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Spin))]
public class Coin : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Coin Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<Spin>().Init();
        return this;
    }

    public void Pick() {
        //TODO: play sound
        gameObject.Destroy();
    }
}