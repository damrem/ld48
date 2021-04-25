using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(Picker))]
[RequireComponent(typeof(Walker))]
public class Player : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public Walker Walker { get; private set; }
    public PlayerControl Control { get; private set; }

    public Player Init(Cell cell) {
        Control = GetComponent<PlayerControl>().Init();
        Walker = GetComponent<Walker>().Init(cell);
        return this;
    }

    public void Clear() {
        Walker.Clear();
        Destroy(gameObject);
    }
}
