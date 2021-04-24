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
    Exit ExitPrefab;

    PRNG PRNG;
    Block[,] Blocks;
    public Level Init(int index, LevelDef def, Block blockPrefab, Exit exitPrefab, int seed, Color[] colors) {
        Def = def;
        PRNG = new PRNG(seed);
        BlockPrefab = blockPrefab;
        ExitPrefab = exitPrefab;
        Colors = colors;
        Blocks = new Block[def.Width, def.Depth + 2];
        Blocks.Fill(CreateBlock);

        Blocks.GetRow(0).ToList().ForEach(DestroyBlock);
        AddBottom();

        AddExit();

        return this;
    }

    void AddBottom() {
        Blocks.GetRow(Def.Depth + 1).ToList().ForEach(block => {
            block.SetUnbreakable();
        });
    }

    void AddExit() {
        var exit = Instantiate(ExitPrefab);
        exit.transform.SetParent(transform);
        var cell = new Cell(PRNG.Int(Def.Width), Def.Depth);
        DestroyBlock(cell);
        exit.GetComponent<Exit>().Init(cell);
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
        if (block.IsUnbreakable) return;

        DestroyBlock(block.Cell);
    }

    public Block[] GetColumn(int x) {
        return Blocks.GetColumn(x);
    }
}