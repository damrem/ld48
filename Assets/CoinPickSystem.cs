using UnityEngine;

public class CoinPickSystem : MonoBehaviour {
    Level Level;

    public CoinPickSystem Init(Level level, Picker picker) {
        picker.OnTouchedPickable += Pick;
        Level = level;
        return this;
    }

    void Pick(Pickable pickable) {
        switch (pickable.Type) {
            case PickableType.Coin:
                var coin = pickable.GetComponent<Coin>();
                Level.RemoveCoin(coin);
                break;

            case PickableType.Gem:
                var gem = pickable.GetComponent<Gem>();
                Level.RemoveGem(gem);
                break;

            default: break;
        }
        pickable.Pick();
    }
}