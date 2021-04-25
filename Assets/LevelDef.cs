using System;
using UnityEngine;

[Serializable]
public struct LevelDef {
    public int Width;
    public int Depth;
    [Range(.25f, 1)] public float BlockDensity;
    [Range(0, 1)] public float CoinDensity;
    [Range(0, 1)] public float GemDensity;

    public static LevelDef CreateLevelDef(int index) {
        var w = index + 3;
        return new LevelDef {
            Width = w,
            Depth = w * w,
            BlockDensity = 1f / 3f + (float)index / 300f,
            CoinDensity = (float)(100f - index) / 100f * .25f,
            GemDensity = (float)(100f - index) / 100f * .1f,
        };
    }
}