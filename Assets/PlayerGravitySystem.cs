using System.Linq;
using Damrem.Collections;
using UnityEngine;

public class PlayerGravitySystem : MonoBehaviour {
    Level Level;
    Player Player;
    public PlayerGravitySystem Init(Level level, Player player) {
        Level = level;
        Player = player;
        player.Walker.OnMoved += CheckFall;
        level.OnBlockDestroyed += CheckFall;
        return this;
    }

    void CheckFall() {
        var blocksUnderPlayer = Level.GetColumn(Player.Cell.X).ToList().GetRangeFrom(Player.Cell.Y);
        Cell targetCell = Player.Cell;
        for (int y = 0; y < blocksUnderPlayer.Count; y++) {
            var currentBlock = blocksUnderPlayer[y];
            if (currentBlock) {
                targetCell = new Cell(currentBlock.Cell.X, currentBlock.Cell.Y - 1);
                break;
            }
        }
        if (targetCell == Player.Cell) return;

        Player.Walker.MoveToCell(targetCell);
    }

    void CheckFall(MoveType moveType) {
        CheckFall();
    }
}