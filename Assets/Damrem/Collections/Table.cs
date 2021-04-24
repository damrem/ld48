using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Collections {
    [Serializable]
    public class Table<T> : IEnumerable<T> {
        T[] Array;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Func<int, int, T> CreateCellContent { get; private set; }

        public Table(int width, int height) {
            Construct(width, height);
        }

        public Table(int width, int height, Func<int, int, T> createCellContent) {
            Construct(width, height);
            CreateCellContent = createCellContent;
        }

        void Construct(int width, int height) {
            Width = width;
            Height = height;
            Array = new T[Width * Height];
        }

        public Vector2Int Coord(int index) {
            return new Vector2Int(index % Width, index / Width);
        }

        public Vector2Int Coord(T item) {
            return Coord(Array.ToList().IndexOf(item));
        }

        public int Index(int x, int y) {
            return x + y * Width;
        }

        public int Index(Vector2Int coord) {
            return Index(coord.x, coord.y);
        }

        public void Set(int index, T item) {
            Array[index] = item;
        }

        public void Set(int x, int y, T item) {
            Set(Index(x, y), item);
        }

        public void Set(Vector2Int v, T item) {
            Set(v.x, v.y, item);
        }

        public T Get(int index) {
            return Array[index];
        }

        public T Get(int x, int y) {
            return Get(Index(x, y));
        }

        public T Get(Vector2Int v) {
            return Get(v.x, v.y);
        }

        public void Clear() {
            Construct(Width, Height);
        }

        public IEnumerator<T> GetEnumerator() { return ((IEnumerable<T>)Array).GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return Array.GetEnumerator(); }

        public List<T> FindAll(Predicate<T> match) {
            var items = new List<T>();
            foreach (var item in Array)
                if (match.Invoke(item))
                    items.Add(item);
            return items;
        }

        public int Count(Predicate<T> match) {
            return Array.FindAll(match).Length;
        }

        public List<T> FindAll(Predicate<T> match, out List<int> indexes) {
            var items = new List<T>();
            var tmpIndexes = new List<int>();
            ForEach((item, i) => {
                if (match.Invoke(item)) {
                    items.Add(item);
                    tmpIndexes.Add(i);
                }
            });
            indexes = tmpIndexes;
            return items;
        }

        public List<T> FindAll(Predicate<T> match, out List<Vector2Int> coords) {
            var items = new List<T>();
            var tmpCoords = new List<Vector2Int>();
            ForEach((item, x, y) => {
                if (match.Invoke(item)) {
                    items.Add(item);
                    tmpCoords.Add(new Vector2Int(x, y));
                }
            });
            coords = tmpCoords;
            return items;
        }

        public List<int> FindAllIndexes(Predicate<T> match) {
            FindAll(match, out List<int> indexes);
            return indexes;
        }

        public List<Vector2Int> FindAllCoords(Predicate<T> match) {
            FindAll(match, out List<Vector2Int> coords);
            return coords;
        }

        public List<Vector2Int> FindAllCoords(List<T> items) {
            FindAll(items.Contains, out List<Vector2Int> coords);
            return coords;
        }

        public void ForEach(Action<T> action) {
            for (var i = 0; i < Array.Length; i++) action.Invoke(Get(i));
        }

        public void ForEach(Action<T, int> action) {
            for (var i = 0; i < Array.Length; i++) action.Invoke(Get(i), i);
        }

        public void ForEach(Action<T, int, int> action) {
            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    action.Invoke(Get(x, y), x, y);
        }

        public void ForEach(Action<T, Vector2Int> action) {
            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    action.Invoke(Get(x, y), new Vector2Int(x, y));
        }

        public bool IsInside(int x, int y) {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsInside(Vector2Int coord) {
            return IsInside(coord.x, coord.y);
        }

        public bool IsInside(int index) {
            return IsInside(Coord(index));
        }

        public bool IsOutside(int x, int y) {
            return !IsInside(x, y);
        }

        public bool IsOutside(Vector2Int coord) {
            return IsOutside(coord.x, coord.y);
        }

        public List<Vector2Int> SquareAround(int x, int y, int size = 1) {
            var list = new List<Vector2Int>();
            for (int xx = -size; xx <= size; xx++)
                for (int yy = -size; yy <= size; yy++)
                    list.Add(new Vector2Int(x + xx, y + yy));
            return list.FindAll(IsInside);
        }

        public List<Vector2Int> SquareAround(Vector2Int coord, int size = 1) {
            return SquareAround(coord.x, coord.y, size);
        }

        public List<Vector2Int> CrossAround(int x, int y, int size = 1) {
            var list = new List<Vector2Int>();

            for (int xx = -size; xx <= size; xx++)
                list.Add(new Vector2Int(x + xx, y));

            for (int yy = -size; yy <= size; yy++)
                list.Add(new Vector2Int(x, y + yy));

            return list.FindAll(IsInside);
        }

        public List<Vector2Int> CrossAround(Vector2Int coord, int size = 1) {
            return CrossAround(coord.x, coord.y, size);
        }

        public List<Vector2Int> LeftCoords(int x, int y) {
            var coords = new List<Vector2Int>();
            for (int xx = x - 1; xx >= 0; xx--)
                coords.Add(new Vector2Int(xx, y));
            return coords;
        }

        public List<Vector2Int> LeftCoords(Vector2Int coord) {
            return LeftCoords(coord.x, coord.y);
        }

        public List<Vector2Int> RightCoords(int x, int y) {
            var coords = new List<Vector2Int>();
            for (int xx = x + 1; xx < Width; xx++)
                coords.Add(new Vector2Int(xx, y));
            return coords;
        }

        public List<Vector2Int> RightCoords(Vector2Int coord) {
            return RightCoords(coord.x, coord.y);
        }

        public List<Vector2Int> DownCoords(int x, int y) {
            var coords = new List<Vector2Int>();
            for (int yy = y - 1; yy >= 0; yy--)
                coords.Add(new Vector2Int(x, yy));
            return coords;
        }

        public List<Vector2Int> DownCoords(Vector2Int coord) {
            return DownCoords(coord.x, coord.y);
        }

        public List<Vector2Int> UpCoords(int x, int y) {
            var coords = new List<Vector2Int>();
            for (int yy = y + 1; yy < Height; yy++)
                coords.Add(new Vector2Int(x, yy));
            return coords;
        }

        public List<Vector2Int> UpCoords(Vector2Int coord) {
            return UpCoords(coord.x, coord.y);
        }

        int FindX(Vector2Int item) { return item.x; }
        int FindY(Vector2Int item) { return item.y; }

        public RectInt Bounds(List<T> items) {
            var indexes = items.Map(item => Array.ToList().IndexOf(item));
            var coords = indexes.Map(Coord);

            coords.MinByInt(FindX, out var left);
            coords.MaxByInt(FindX, out var right);
            coords.MinByInt(FindY, out var bottom);
            coords.MaxByInt(FindY, out var top);

            return new RectInt(left, bottom, right - left, top - bottom);
        }
    }
}