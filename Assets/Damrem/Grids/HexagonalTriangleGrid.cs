using UnityEngine;
using Damrem.DataStructures;
using Damrem.Geom2D;
using Damrem.UnityEngine;
using System.Collections.Generic;

namespace Damrem.Grids {

    public class HexagonalTriangleGrid : ITriangleGrid {

        public Dictionary<Triangle, List<Triangle>> NeighborsByTriangle { get; } = new Dictionary<Triangle, List<Triangle>>();
        public HashSet<Pair<Triangle>> TriangleNeighborPairs { get; private set; } = new HashSet<Pair<Triangle>>();
        public List<List<Vertex>> CornersByLevel { get; } = new List<List<Vertex>>();
        public List<Triangle> BoundTriangles { get; private set; }
        public List<Triangle> Triangles { get; } = new List<Triangle>();

        readonly List<Vector2> Axises = new List<Vector2>();

        public HexagonalTriangleGrid(Vector2 center, float outerRadius, int levelCount = 2) {
            Axises = FillAxises(outerRadius / levelCount);

            CornersByLevel = new List<List<Vertex>>() { new List<Vertex>() { Vertex.Pool.TakeObject(center) } };

            //var corners = new List<Corner>() { new Corner(center) };
            for (int level = 1; level <= levelCount; level++) {
                CornersByLevel.Add(GetLevelCorners(center, level));
            }

            for (int level = 1; level <= levelCount; level++) {
                var levelCorners = CornersByLevel[level];

                var innerTriangles = GetLevelInnerTriangles(level, levelCorners, CornersByLevel[level - 1]);
                Triangles.AddRange(innerTriangles);
                if (level == levelCount) BoundTriangles = innerTriangles;

                if (level < levelCount) Triangles.AddRange(GetLevelOuterTriangles(level, levelCorners, CornersByLevel[level + 1]));
            }

            //corners = cornersByLevel.GetFlattened();
            ComputeNeighboring();
        }

        void ComputeNeighboring() {
            var uncomputeds = new List<Triangle>(Triangles);
            foreach (var t in Triangles) {
                uncomputeds.Remove(t);
                foreach (var u in uncomputeds) {
                    if (t.IsAdjacentTo(u)) {
                        TriangleNeighborPairs.Add(new Pair<Triangle>(t, u));
                        NeighborsByTriangle.AddToKey(t, u);
                        NeighborsByTriangle.AddToKey(u, t);
                    }
                }
            }
        }


        List<Vector2> FillAxises(float length) {
            var axises = new List<Vector2>() { new Vector2(length, 0) };
            for (int i = 1; i < 6; i++)
                axises.Add(axises[i - 1].Rotate(60));
            return axises;
        }

        List<Vertex> GetLevelCorners(Vector2 center, int level) {
            var corners = new List<Vertex>();

            for (int axisIndex = 0; axisIndex < 6; axisIndex++) {
                var pos = center + Axises[axisIndex] * level;
                corners.Add(Vertex.Pool.TakeObject(pos));
                for (int j = 1; j < level; j++) {
                    var nextAxis = Axises[(axisIndex + 2) % 6];
                    corners.Add(Vertex.Pool.TakeObject(pos + nextAxis * j));
                }
            }

            return corners;
        }

        List<Triangle> GetLevelInnerTriangles(int level, List<Vertex> levelCorners, List<Vertex> prevLevelCorners) {
            var triangles = new List<Triangle>();
            int prevLevelIndex = 0;

            for (int i = 0; i < levelCorners.Count; i++) {
                var nextI = (i + 1) % levelCorners.Count;
                var a = levelCorners[i];
                var b = levelCorners[nextI];
                if (i % level != 0) {
                    prevLevelIndex++;
                    prevLevelIndex %= prevLevelCorners.Count;
                }
                var c = prevLevelCorners[prevLevelIndex];
                triangles.Add(new Triangle(new List<Vertex>() { a, b, c }));
            }
            return triangles;
        }

        List<Triangle> GetLevelOuterTriangles(int level, List<Vertex> levelCorners, List<Vertex> nextLevelCorners) {
            var triangles = new List<Triangle>();
            int nextLevel = level + 1;
            int nextLevelIndex = 0;

            for (int i = 0; i < levelCorners.Count; i++) {
                var nextI = (i + 1) % levelCorners.Count;
                var a = levelCorners[i];
                var b = levelCorners[nextI];
                nextLevelIndex++;
                if (nextLevelIndex % nextLevel == 0) {
                    nextLevelIndex++;
                }
                nextLevelIndex %= nextLevelCorners.Count;
                var c = nextLevelCorners[nextLevelIndex];
                triangles.Add(new Triangle(new List<Vertex>() { a, b, c }));
            }
            return triangles;
        }


    }
}
