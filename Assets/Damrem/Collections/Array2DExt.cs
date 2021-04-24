using Damrem.Procedural;
using System;
using System.Collections.Generic;

namespace Damrem.Collections {

    public static class Array2DExt {

        public static U[,] Map<T, U>(this T[,] array, Func<T, U> callback) {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            var output = new U[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    output[x, y] = callback(array[x, y]);

            return output;
        }

        public static string Join<T>(this T[,] array, string separator = ";", string superSeparator = "\n") {
            string r = "";
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    T item = array[i, j];
                    r += (item != null) ? item.ToString() : "null";
                    r += separator;
                }
                r = r.Remove(r.Length - 1 - separator.Length);
                r += superSeparator;
            }
            r = r.Remove(r.Length - 1 - superSeparator.Length);
            return r;
        }

        public static T[] Filter<T>(this T[,] array2d, Func<T, bool> callback) {
            var filteredList = new List<T>();
            foreach (T item in array2d)
                if (callback(item))
                    filteredList.Add(item);
            return filteredList.ToArray();
        }

        public static void Fill<T>(this T[,] array2d, Func<int, int, T> filler) {
            for (int x = 0; x < array2d.GetLength(0); x++)
                for (int y = 0; y < array2d.GetLength(1); y++)
                    array2d[x, y] = filler.Invoke(x, y);
        }

        [Obsolete("should be moved to Grids")]
        public static bool[,] Smooth(this bool[,] array, PRNG prng, int minNeighborCountToAdd = 4, int maxNeighborCountToRemove = 4, float probabilityToAdd = 1f, float probabilityToRemove = 1f) {
            bool[,] smoothed = (bool[,])array.Clone();
            for (int x = 0; x < smoothed.GetLength(0); x++) {
                for (int y = 0; y < smoothed.GetLength(1); y++) {
                    int neighbourWallTiles = array.GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > minNeighborCountToAdd && prng.Bool(probabilityToAdd))
                        smoothed[x, y] = true;
                    else if (neighbourWallTiles < maxNeighborCountToRemove && prng.Bool(probabilityToRemove))
                        smoothed[x, y] = false;

                }
            }
            return smoothed;
        }

        [Obsolete("should be moved to Grids")]
        static int GetSurroundingWallCount(this bool[,] array, int gridX, int gridY) {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                    if (neighbourX >= 0 && neighbourX < array.GetLength(0) && neighbourY >= 0 && neighbourY < array.GetLength(1)) {
                        if (neighbourX != gridX || neighbourY != gridY) {
                            wallCount += array[neighbourX, neighbourY] ? 1 : 0;
                        }
                    }
                    else {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        public static T[] GetColumn<T>(this T[,] array, int x) {
            var height = array.GetLength(1);
            var column = new T[height];

            for (int y = 0; y < height; y++)
                column[y] = array[x, y];

            return column;
        }

        public static T[] GetRow<T>(this T[,] array, int y) {
            var width = array.GetLength(0);
            var row = new T[width];

            for (int x = 0; x < width; x++)
                row[x] = array[x, y];

            return row;
        }

        public static void ForEach<T>(this T[,] array, Action<T> callback) {
            for (int x = 0; x < array.GetLength(0); x++) {
                for (int y = 0; y < array.GetLength(1); y++) {
                    callback(array[x, y]);
                }
            }
        }

        public static void ForEach<T>(this T[,] array, Action<T, int, int> callback) {
            for (int x = 0; x < array.GetLength(0); x++) {
                for (int y = 0; y < array.GetLength(1); y++) {
                    callback(array[x, y], x, y);
                }
            }
        }

        public static T[] GetFlattened<T>(this T[,] array) {
            var flat = new List<T>();
            array.ForEach(flat.Add);
            return flat.ToArray();
        }
    }
}
