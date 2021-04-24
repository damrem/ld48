using System;
using Damrem.Collections;
using Damrem.Procedural;
using UnityEngine;

public class Level : MonoBehaviour {
    public event Action OnBlockDestroyed;
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
        Blocks.GetRow(0).ToList().ForEach(DestroyBlock);

        return this;
    }

    Block CreateBlock(int x, int y) {
        var block = Instantiate(BlockPrefab).Init(new Cell(x, y), PRNG.InArray(Colors));
        block.name = $"Block-{x}-{y}";
        block.transform.parent = transform;
        return block;
    }

    public Block GetBlock(Cell cell) {
        return Blocks[cell.X, cell.Y];
    }

    void DestroyBlock(Cell cell) {
        var block = GetBlock(cell);
        Blocks[cell.X, cell.Y] = null;
        Destroy(block.gameObject);
        OnBlockDestroyed?.Invoke();
    }

    public void DestroyBlock(Block block) {
        DestroyBlock(block.Cell);
    }

    public Block[] GetColumn(int x) {
        return Blocks.GetColumn(x);
    }
}