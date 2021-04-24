using Damrem.Collections;
using UnityEngine;

public class PlayerGravitySystem : MonoBehaviour {
    Level Level;
    Player Player;
    public PlayerGravitySystem Init(Level level, Player player) {
        Level = level;
        Player = player;
        level.OnBlockDestroyed += OnBlockDestroyed;
        return this;
    }

    void OnBlockDestroyed() {
        var col = Level.GetColumn(Player.Cell.X);

        var blocksUnderPlayer = col.ToList().GetRangeFrom(Player.Cell.Y);

        Cell targetCell = Player.Cell;
        for (int y = Player.Cell.Y + 1; y < col.Length; y++) {
            var currentBlock = blocksUnderPlayer[y];
            if (currentBlock) {
                targetCell = new Cell(Player.Cell.X, y - 1);
                break;
            }
        }

        if (targetCell == Player.Cell) return;

        Player.MoveToCell(targetCell);
    }
}