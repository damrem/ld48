using System;
using System.Collections.Generic;
using System.Linq;
using Damrem.Collections;
using Damrem.Procedural;
using UnityEngine;

[RequireComponent(typeof(BlockGroupSystem))]
public class Level : MonoBehaviour {
    public event Action OnBlockDestroyed;
    public event Action<int> OnGroupDestroyed;
    public LevelDef Def { get; private set; }
    public Exit Exit { get; private set; }
    public Coin[,] Coins { get; private set; }
    public Gem[,] Gems { get; private set; }
    Color[] Colors;
    Block BlockPrefab;
    Exit ExitPrefab;
    Coin CoinPrefab;
    Gem GemPrefab;
    PRNG PRNG;
    Block[,] Blocks;
    BlockGroupSystem BlockGroupSystem;
    public Level Init(LevelDef def, Block blockPrefab, Exit exitPrefab, Coin coinPrefab, Gem gemPrefab, int seed, Color[] colors) {
        Def = def;

        PRNG = new PRNG();

        BlockPrefab = blockPrefab;
        ExitPrefab = exitPrefab;
        CoinPrefab = coinPrefab;
        GemPrefab = gemPrefab;
        Colors = PRNG.Shuffle(colors, Def.ColorCount);

        Blocks = new Block[def.Width, def.Depth + 2];
        Blocks.Fill((x, y) => CreateBlock(x, y));

        Blocks.GetRow(0).ToList()
        .FindAll(block => block != null)
        .ForEach(block => {
            DestroyBlock(block, false);
        });

        CreateBottom();
        Exit = CreateExit();

        BlockGroupSystem = GetComponent<BlockGroupSystem>().Init(this);

        AddGemRandomlyInRows();

        Coins = new Coin[def.Width, def.Depth + 2];
        Coins.Fill(CreateCoin);
        Coins.ForEach(item => {
            if (!item) return;
            DestroyBlock(item.Cell, false);
        });

        return this;
    }

    void AddGemRandomlyInCells() {
        Gems = new Gem[Def.Width, Def.Depth + 2];
        Gems.Fill(CreateGem);
        Gems.ForEach(item => {
            if (!item) return;
            DestroyBlock(item.Cell, false);
        });
    }

    void AddGemRandomlyInRows() {
        Gems = new Gem[Def.Width, Def.Depth + 2];

        int spacing = Def.Index * 2;

        for (int y = 0; y <= Gems.GetLength(1); y += spacing) {
            spacing++;
            var x = PRNG.Int(Def.Width);
            Gems[x, y] = CreateGem(x, y);
        }

        Gems.ForEach(item => {
            if (!item) return;
            DestroyBlock(item.Cell, false);
        });
    }

    public void AddBlockUnderPlayer(int playerInitialX) {
        if (GetBlock(new Cell(playerInitialX, 1))) return;

        Blocks[playerInitialX, 1] = CreateBlock(playerInitialX, 1, true);
    }

    public void Clear() {
        OnBlockDestroyed = default;
        OnGroupDestroyed = default;
        Destroy(gameObject);
    }

    Coin CreateCoin(int x, int y) {
        if (y == 0) return null;
        if (!PRNG.Bool(Def.CoinDensity)) return null;

        var cell = new Cell(x, y);
        if (cell == Exit.Cell) return null;
        // if (GetBlock(cell)) return null;
        // if (!GetBlock(cell + Vector2Int.up)) return null;
        if (Gems != null && GetGem(cell)) return null;

        return Instantiate(CoinPrefab, transform).Init(cell);
    }

    Gem CreateGem(int x, int y) {
        if (y == 0) return null;
        // if (!PRNG.Bool(Def.GemDensity)) return null;

        var cell = new Cell(x, y);
        if (cell == Exit.Cell) return null;
        // if (GetBlock(cell)) return null;
        // if (!GetBlock(cell + Vector2Int.up)) return null;
        // if (Coins != null && GetCoin(cell)) return null;
        return Instantiate(GemPrefab, transform).Init(cell);
    }

    IEnumerable<Block> CreateBottom() {
        Blocks.GetRow(Def.Depth + 1).ToList()
        .FindAll(block => block != null)
        .ForEach(block => {
            DestroyBlock(block, false);
        });

        for (int x = 0; x < Def.Width; x++) {
            var block = CreateBlock(x, Def.Depth + 1, true);
            block.SetUnbreakable();
            Blocks[x, Def.Depth + 1] = block;
        }
        return Blocks.GetRow(Blocks.GetLength(1) - 1);
    }

    public void RemoveCoin(Coin item) {
        Coins[item.Cell.X, item.Cell.Y] = null;
    }

    public void RemoveGem(Gem item) {
        Gems[item.Cell.X, item.Cell.Y] = null;
    }

    public Coin GetCoin(Cell cell) {
        if (Coins == null) return null;
        return Coins[cell.X, cell.Y];
    }

    public Gem GetGem(Cell cell) {
        if (Gems == null) return null;
        return Gems[cell.X, cell.Y];
    }

    Exit CreateExit() {
        var exit = Instantiate(ExitPrefab);
        exit.transform.SetParent(transform);
        var cell = new Cell(PRNG.Int(Def.Width), Def.Depth);
        DestroyBlock(cell);
        return exit.GetComponent<Exit>().Init(cell);
    }

    Block CreateBlock(int x, int y, bool force = false) {
        // if (!force && !PRNG.Bool(Def.BlockDensity)) return null;

        var type = PRNG.Int(Colors.Length);
        var block = Instantiate(BlockPrefab).Init(new Cell(x, y), type, Colors[type]);
        block.name = $"Block-{x}-{y}";
        block.transform.parent = transform;
        return block;
    }

    public Block GetBlock(Cell cell) {
        return Blocks[cell.X, cell.Y];
    }

    void DestroyBlock(Cell cell, bool animate = true) {
        var block = GetBlock(cell);
        if (!block) return;
        if (block.IsUnbreakable) return;

        Blocks[cell.X, cell.Y] = null;

        void onEnd(Block block) {
            Destroy(block.gameObject);
            OnBlockDestroyed?.Invoke();
        };
        if (animate) block.AnimateDestroy(onEnd);
        else onEnd(block);
    }

    public void DestroyBlock(Block block, bool animate = true) {
        DestroyBlock(block.Cell, animate);
    }

    public void UnhighlightAll() {
        Blocks.ForEach(block => {
            block?.Highlight(false);
        });
    }

    public void HighlightGroup(Cell cell, bool toggle) {
        if (cell.X < 0) return;
        if (cell.X >= Def.Width) return;
        if (cell.Y < 0) return;
        if (cell.Y > Def.Width + 1) return;
        var block = GetBlock(cell);
        HighlightGroup(block, toggle);
    }

    void HighlightGroup(Block block, bool toggle) {
        if (!block) return;//why would it be null???
        if (block.IsUnbreakable) return;

        var group = BlockGroupSystem.GroupFrom(block).ToList();

        group.ForEach(block => {
            block.Highlight(toggle);
        });
    }

    public void DestroyGroup(Block block) {
        if (!block) return;//why would it be null???
        if (block.IsUnbreakable) return;

        var group = BlockGroupSystem.GroupFrom(block).ToList();

        group.ForEach(block => {
            DestroyBlock(block, true);
        });

        OnGroupDestroyed?.Invoke(group.Count);
    }

    public Block[] GetColumn(int x) {
        return Blocks.GetColumn(x);
    }

    public Block LeftBlock(Cell cell) {
        if (cell.X <= 0) return null;

        return GetBlock(cell + Vector2Int.left);
    }

    public Block RightBlock(Cell cell) {
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