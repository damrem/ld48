using System.Collections.Generic;
using Damrem.Collections;
using Damrem.Procedural;
using UnityEngine;

public class Level : MonoBehaviour {
    public Color[] Colors;
    public Block BlockPrefab;
    public int Width;
    public int Height;
    public int Seed = 0;

    PRNG PRNG;
    Block[,] Blocks;
    public Level Init() {
        PRNG = new PRNG(Seed);
        Blocks = new Block[Width, Height];
        Blocks.Fill(CreateBlock);
        return this;
    }

    Block CreateBlock(int x, int y) {
        var block = Instantiate(BlockPrefab).Init(PRNG.InArray(Colors));
        block.name = $"Block-{x}-{y}";
        block.transform.parent = transform;
        block.transform.position = new Vector2(x, y);
        return block;
    }
}