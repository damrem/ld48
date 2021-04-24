using System;
using UnityEngine;

public class CoinPickSystem : MonoBehaviour {
    Level Level;
    Player Player;
    Action OnTouched;

    public CoinPickSystem Init(Level level, Player player, Action onTouched) {
        Level = level;
        Player = player;
        OnTouched = onTouched;
        player.OnMoved += CheckIfHasTouched;
        return this;
    }

    void CheckIfHasTouched(MoveType _) {
        Debug.Log("CheckIfHasTouched");
        var item = Level.GetCoin(Player.Cell);
        if (!item) return;

        Pick(item);
    }

    void Pick(Coin coin) {
        Level.RemoveCoin(coin);
        coin.Pick();
        OnTouched?.Invoke();

    }
}