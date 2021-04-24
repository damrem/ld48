using System;
using Damrem.Collections;
using UnityEngine;

public class PlayerPickCoinSystem : MonoBehaviour {
    Level Level;
    Player Player;
    Action OnPlayerTouchedCoin;

    public PlayerPickCoinSystem Init(Level level, Player player, Action onPlayerTouchedCoin) {
        Level = level;
        Player = player;
        OnPlayerTouchedCoin = onPlayerTouchedCoin;
        player.OnMoved += CheckIfPlayerHasTouchedCoin;
        return this;
    }

    void CheckIfPlayerHasTouchedCoin(MoveType _) {
        Debug.Log("CheckIfPlayerHasTouchedCoin");
        var coin = Level.GetCoin(Player.Cell);
        Debug.Log("coin " + coin);
        if (!coin) return;

        PickCoin(coin);
    }

    void PickCoin(Coin coin) {
        Level.RemoveCoin(coin);
        coin.Pick();
        OnPlayerTouchedCoin?.Invoke();

    }
}