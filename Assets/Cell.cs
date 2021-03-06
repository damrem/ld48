using Damrem.System;
using UnityEngine;

public struct Cell {
    public readonly int X;
    public readonly int Y;
    public Cell(int x, int y) {
        X = x;
        Y = y;
    }

    public Cell(Vector2Int position) {
        X = position.x;
        Y = position.y;
    }

    public Vector3 ToWorldPosition() {
        return new Vector3(X, -Y);
    }

    public static Cell operator +(Cell a, Vector2Int b) {
        return new Cell(a.X + b.x, a.Y + b.y);
    }

    public override bool Equals(object obj) {
        return this == (Cell)obj;
    }

    public static bool operator ==(Cell a, Cell b) {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Cell a, Cell b) {
        return a.X != b.X || a.Y != b.Y;
    }

    public override string ToString() {
        return $"{base.ToString()}: X={X}, Y={Y}";
    }

    public override int GetHashCode() {
        return HashCodeHelper.CombineHashCodes(X, Y);
    }
}