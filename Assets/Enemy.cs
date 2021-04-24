using UnityEngine;

[RequireComponent(typeof(CellPosition))]
public class Enemy : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    EnemyState State = EnemyState.Patrol;
    Level Level;
    Player Player;
    public Enemy Init(Cell cell, Level level, Player player) {
        Level = level;
        Player = player;
        return this;
    }

    void Update() {
        switch (State) {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Follow:
                Follow();
                break;

            default: break;
        }
    }

    void Follow() {
        if (Player.Cell.X < Cell.X) { }
    }

    void Patrol() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) State = EnemyState.Follow;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Player>()) State = EnemyState.Patrol;
    }
}