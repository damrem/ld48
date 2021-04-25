using System;
using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour {
    public event Action<EnergySpendingType> OnEnergySpent;
    Level Level;
    Player Player;
    bool IsDigging = false;
    public PlayerMovementSystem Init(Level level, Player player, Action<EnergySpendingType> onEnergySpent) {
        Level = level;
        Player = player;
        // player.Walker.OnMovementRequired += AttemptWalk;
        OnEnergySpent = onEnergySpent;
        return this;
    }

    void Update() {
        if (Player is null) return;
        if (Player.Walker is null) return;
        if (Player.Walker.IsMoving || IsDigging) return;

        switch (Player.Control.InputDirection) {
            default: break;

            case Direction.Left:
                AttemptWalk(Vector2Int.left);
                break;

            case Direction.Right:
                AttemptWalk(Vector2Int.right);
                break;

            case Direction.Down:
                AttemptDig(Vector2Int.up);
                break;
        }
    }

    void AttemptWalk(Vector2Int direction) {
        Debug.Log("AttemptWalk " + direction);
        if (direction.y < 0) return;

        var targetCell = Player.Cell + direction;
        if (targetCell.X < 0 || targetCell.X >= Level.Def.Width) return;

        var targetBlock = Level.GetBlock(targetCell);

        if (targetBlock) {
            AttemptDig(direction);
        }
        else {
            OnEnergySpent.Invoke(EnergySpendingType.Walk);
            Player.Walker.MoveToCell(targetCell);
        }
    }

    void AttemptDig(Vector2Int direction) {
        var targetCell = Player.Cell + direction;
        if (targetCell.Y > Level.Def.Depth) return;

        IsDigging = true;
        var targetBlock = Level.GetBlock(targetCell);
        OnEnergySpent.Invoke(EnergySpendingType.Dig);
        Level.DestroyGroup(targetBlock);
        Invoke("StopDig", .5f);
    }

    void StopDig() {
        IsDigging = false;
    }
}