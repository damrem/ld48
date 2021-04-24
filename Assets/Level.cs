using System;
using Damrem.Collections;
using Damrem.Procedural;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementSystem))]
[RequireComponent(typeof(PlayerGravitySystem))]
public class Level : MonoBehaviour {
    public event Action OnBlockDestroyed;
    public event Action OnComplete;
    public LevelDef Def { get; private set; }
    Color[] Colors;
    Block BlockPrefab;

    PRNG PRNG;
    Block[,] Blocks;
    public Level Init(int index, LevelDef def, Block blockPrefab, int seed, Color[] colors) {
        Def = def;
        PRNG = new PRNG(seed);
        BlockPrefab = blockPrefab;
        Colors = colors;
        Blocks = new Block[def.Width, def.Depth];
        Blocks.Fill(CreateBlock);

        Blocks.GetRow(0).ToList().ForEach(DestroyBlock);

        AddStair();

        return this;
    }

    void AddStair() {

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