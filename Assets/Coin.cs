using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(Spin))]
public class Coin : MonoBehaviour {
    public Coin Init() {
        return this;
    }
}