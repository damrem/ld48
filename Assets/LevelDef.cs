using System;
using UnityEngine;

[Serializable]
public class LevelDef {
    public int Width = 7;
    public int Depth = 49;
    [Range(.25f, 1)] public float BlockDensity;
    [Range(0, 1)] public float CoinDensity = .1f;
    [Range(0, 1)] public float GemDensity = .1f;
}