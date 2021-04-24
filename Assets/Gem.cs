using Damrem.UnityEngine;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Spin))]
public class Gem : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Gem Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        GetComponent<Spin>().Init();
        return this;
    }

    public void Pick() {
        //TODO: play sound
        gameObject.Destroy();
    }
}