using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Spin))]
public class Coin : MonoBehaviour {
    public Coin Init() {
        GetComponent<Spin>().Init();
        return this;
    }
}