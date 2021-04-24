using System.Collections.Generic;
using UnityEngine;

public class BlockGroupSystem : MonoBehaviour {
    Level Level;

    public BlockGroupSystem Init(Level level) {
        Level = level;
        return this;
    }

    public HashSet<Block> GroupFrom(Block block) {
        Debug.Log("GroupFrom " + block);
        var blocks = new HashSet<Block>();
        GroupFrom(block.Cell, block.Type, blocks, null);
        return blocks;
    }

    void GroupFrom(Cell cell, int type, HashSet<Block> blocks = null, HashSet<Block> already = null) {
        blocks = blocks is null ? new HashSet<Block>() : blocks;
        already = already is null ? new HashSet<Block>() : already;

        var block = Level.GetBlock(cell);
        blocks.Add(block);
        already.Add(block);

        var neighbors = GetNeighbors(cell).FindAll(c => c.Type == type);
        blocks.UnionWith(neighbors);

        foreach (var neighbor in neighbors.FindAll(n => !already.Contains(n))) {
            already.Add(neighbor);
            GroupFrom(neighbor.Cell, neighbor.Type, blocks, already);
        }
    }

    List<Block> GetNeighbors(Cell cell) {
        var neighbors = new List<Block> {
            Level.LeftBlock(cell),
            Level.RightBlock(cell),
            Level.DownBlock(cell),
            Level.UpBlock(cell),
        };
        return neighbors.FindAll(c => c != null);
    }
}