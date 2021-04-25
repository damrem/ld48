using UnityEngine;

[System.Serializable]
public struct LevelDef {
    public int Index;
    public int Width;
    public int Depth;
    [Range(.25f, 1)] public float BlockDensity;
    [Range(0, 1)] public float CoinDensity;
    public int GemVerticalSpacing;
    public int ColorCount;

    public static LevelDef CreateLevelDef(int index, float coinDensity, Color[] colors) {
        var w = (index + 4) * 1.5f;
        return new LevelDef {
            Index = index,
            Width = (int)w,
            Depth = (int)w * (int)w,
            BlockDensity = .5f + Random.Range(0, .5f),
            // CoinDensity = (float)(100f - index) / 100f * .25f,
            // GemDensity = (float)(100f - index) / 100f * .1f,
            CoinDensity = coinDensity,
            GemVerticalSpacing = Mathf.RoundToInt((index + 3) * 1.25f),
            ColorCount = Mathf.Min(index + 2, colors.Length),
        };
    }
}