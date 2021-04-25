using System;
using UnityEngine;

[RequireComponent(typeof(Overlay))]
public class GameOverScreen : MonoBehaviour {
    public LevelTitle LevelTitle;
    public Purse Purse;
    public Overlay Overlay { get { return GetComponent<Overlay>(); } }
    public GameOverScreen Init(int levelNumber, int purseValue, Action onInteracted) {
        Debug.Log("GameOverScreen.Init");
        GetComponent<Overlay>().Init(onInteracted);
        LevelTitle.SetLevelIndex(levelNumber - 1);
        Purse.Init(purseValue);
        return this;
    }
}