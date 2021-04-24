using System;
using System.Collections.Generic;
using System.Linq;
using Damrem.Collections;
using Damrem.Procedural;
using UnityEngine;

[RequireComponent(typeof(BlockGroupSystem))]
public class Level : MonoBehaviour {
    public event Action OnBlockDestroyed;
    public LevelDef Def { get; private set; }
    public Exit Exit { get; private set; }
    public Coin[,] Coins { get; private set; }
    Color[] Colors;
    Block BlockPrefab;
    Exit ExitPrefab;
    Coin CoinPrefab;
    PRNG PRNG;
    Block[,] Blocks;
    BlockGroupSystem BlockGroupSystem;
    public Level Init(LevelDef def, Block blockPrefab, Exit exitPrefab, Coin coinPrefab, int seed, Color[] colors) {
        Def = def;
        PRNG = new PRNG(seed);
        BlockPrefab = blockPrefab;
        ExitPrefab = exitPrefab;
        CoinPrefab = coinPrefab;
        Colors = colors;
        Blocks = new Block[def.Width, def.Depth + 2];
        Blocks.Fill((x, y) => CreateBlock(x, y, true));

        Blocks.GetRow(0).ToList().FindAll(block => block != null).ForEach(DestroyBlock);
        CreateBottom();
        Exit = CreateExit();

        BlockGroupSystem = GetComponent<BlockGroupSystem>().Init(this);

        Coins = new Coin[def.Width, def.Depth + 2];
        Coins.Fill(CreateCoin);

        return this;
    }

    public void Clear() {
        OnBlockDestroyed = default;
        Destroy(gameObject);
    }

    Coin CreateCoin(int x, int y) {
        if (y == 0) return null;
        if (!PRNG.Bool(Def.CoinDensity)) return null;

        var cell = new Cell(x, y);
        if (cell == Exit.Cell) return null;

        return Instantiate(CoinPrefab, transform).Init(cell);
    }

    IEnumerable<Block> CreateBottom() {
        Blocks.GetRow(Def.Depth + 1).ToList().FindAll(block => block != null).ForEach(DestroyBlock);
        for (int x = 0; x < Def.Width; x++) {
            var block = CreateBlock(x, Def.Depth + 1, true);
            block.SetUnbreakable();
            Blocks[x, Def.Depth + 1] = block;
        }
        return Blocks.GetRow(Blocks.GetLength(1) - 1);
    }

    public void RemoveCoin(Coin coin) {
        Coins[coin.Cell.X, coin.Cell.Y] = null;
    }

    public Coin GetCoin(Cell cell) {
        return Coins[cell.X, cell.Y];
    }

    Exit CreateExit() {
        var exit = Instantiate(ExitPrefab);
        exit.transform.SetParent(transform);
        var cell = new Cell(PRNG.Int(Def.Width), Def.Depth);
        DestroyBlock(cell);
        return exit.GetComponent<Exit>().Init(cell);
    }

    Block CreateBlock(int x, int y, bool force = false) {
        if (!force && !PRNG.Bool(Def.BlockDensity)) return null;

        var type = PRNG.Int(Colors.Length);
        var block = Instantiate(BlockPrefab).Init(new Cell(x, y), type, Colors[type]);
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

    public void DestroyGroup(Block block) {
        if (block.IsUnbreakable) return;

        var group = BlockGroupSystem.GroupFrom(block).ToList();

        group.ForEach(DestroyBlock);
    }

    public Block[] GetColumn(int x) {
        return Blocks.GetColumn(x);
    }

    public Block LeftBlock(Cell cell) {
        if (cell.X <= 0) return null;

        return GetBlock(cell + Vector2Int.left);
    }

    public Block RightBlock(Cell cell) {
        Debug.Log("RightBlock " + cell);
        Debug.Log("Width " + Def.Width);
        if (cell.X >= Def.Width - 1) return null;

        return GetBlock(cell + Vector2Int.right);
    }

    public Block UpBlock(Cell coord) {
        if (coord.Y <= 0) return null;

        return GetBlock(coord + Vector2Int.down);
    }

    public Block DownBlock(Cell coord) {
        if (coord.Y >= Def.Depth) return null;

        return GetBlock(coord + Vector2Int.up);
    }
}