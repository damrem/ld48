using UnityEngine;

public class GameManager : MonoBehaviour {
    void Start() {
        Init();
    }

    void Init() {
        var level = GetComponentInChildren<Level>().Init();
        var player = GetComponentInChildren<Player>().Init(new Cell(3, 0));
        var playerMovementSystem = GetComponent<PlayerMovementSystem>().Init(level, player);
    }

    void Update() {

    }
}
