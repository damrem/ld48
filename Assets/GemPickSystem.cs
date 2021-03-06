// using System;
// using UnityEngine;

// public class GemPickSystem : MonoBehaviour {
//     Level Level;
//     Player Player;
//     Action OnTouched;

//     public GemPickSystem Init(Level level, Player player, Action onTouched) {
//         Level = level;
//         Player = player;
//         OnTouched = onTouched;
//         player.OnMoved += CheckIfHasTouched;
//         return this;
//     }

//     void CheckIfHasTouched(MoveType _) {
//         var item = Level.GetGem(Player.Cell);
//         if (!item) return;

//         Pick(item);
//     }

//     void Pick(Gem item) {
//         Level.RemoveGem(item);
//         item.Pick();
//         OnTouched?.Invoke();

//     }
// }