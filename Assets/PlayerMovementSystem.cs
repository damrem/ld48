using System;
using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour {
    public event Action<EnergySpendingType> OnEnergySpent;
    Level Level;
    Player Player;
    public PlayerMovementSystem Init(Level level, Player player, Action<EnergySpendingType> onEnergySpent) {
        Level = level;
        Player = player;
        player.OnMovementRequired += OnMovementRequired;
        OnEnergySpent = onEnergySpent;
        return this;
    }

    void OnMovementRequired(Vector2Int direction) {
        if (direction.y < 0) return;

        var targetCell = Player.Cell + direction;
        if (targetCell.X < 0 || targetCell.X >= Level.Def.Width) return;

        var targetBlock = Level.GetBlock(targetCell);

        if (targetBlock) {
            OnEnergySpent.Invoke(EnergySpendingType.Dig);
            Level.DestroyGroup(targetBlock);
        }
        else {
            OnEnergySpent.Invoke(EnergySpendingType.Walk);
            Player.MoveToCell(targetCell);
        }
    }
}