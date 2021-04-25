using System;
using UnityEngine;

public class ExitSystem : MonoBehaviour {
    Level Level;
    Player Player;
    Action OnPlayerReachedExit;
    public ExitSystem Init(Level level, Player player, Action onPlayerReachedExit) {
        Level = level;
        Player = player;
        OnPlayerReachedExit = onPlayerReachedExit;
        Player.Walker.OnMoved += CheckIfPlayerHasExitted;
        return this;
    }

    void CheckIfPlayerHasExitted(MoveType moveType, Cell cell) {
        if (cell != Level.Exit.Cell) return;

        OnPlayerReachedExit?.Invoke();
    }
}