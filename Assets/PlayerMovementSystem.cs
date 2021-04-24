using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour {
    Level Level;
    Player Player;
    public PlayerMovementSystem Init(Level level, Player player) {
        Level = level;
        Player = player;
        return this;
    }
}