using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour {
    Level Level;
    Player Player;
    public PlayerMovementSystem Init(Level level, Player player) {
        Level = level;
        Player = player;
        player.OnMovementRequired += OnMovementRequired;
        return this;
    }

    void OnMovementRequired(Vector2Int direction) {
        var targetCell = Player.Cell + direction;
        Debug.Log((targetCell.X + " " + Level.Width));
        if (targetCell.X < 0 || targetCell.X >= Level.Width) return;

        var targetBlock = Level.GetBlock(targetCell);

        if (targetBlock) Level.DestroyBlock(targetBlock);
        else Player.MoveToCell(targetCell);
    }
}