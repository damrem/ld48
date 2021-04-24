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
        if (direction.y < 0) return;

        var targetCell = Player.Cell + direction;
        if (targetCell.X < 0 || targetCell.X >= Level.Def.Width) return;

        var targetBlock = Level.GetBlock(targetCell);

        if (targetBlock) Level.DestroyGroup(targetBlock);
        else Player.MoveToCell(targetCell);
    }
}