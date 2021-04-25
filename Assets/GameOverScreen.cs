using System;
using UnityEngine;

[RequireComponent(typeof(Overlay))]
public class GameOverScreen : MonoBehaviour {
    public LevelTitle LevelTitle;
    public Purse Purse;
    public Overlay Overlay { get { return GetComponent<Overlay>(); } }
    public GameOverScreen Init(int levelIndex, int purseValue, Action onInteracted) {
        GetComponent<Overlay>().Init(onInteracted);
        LevelTitle.SetLevelIndex(levelIndex);
        Purse.Init(purseValue);
        return this;
    }
}