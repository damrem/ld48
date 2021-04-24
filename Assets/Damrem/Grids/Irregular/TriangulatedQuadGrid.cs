using Damrem.Collections;
using Damrem.DataStructures;
using Damrem.Geom2D;
using Damrem.Grids;
using Damrem.Procedural;
using System.Collections.Generic;
using UnityEngine;

public class TriangulatedQuadGrid : ITriangleGrid {
    readonly PRNG PRNG;

    public List<Vertex> Corners { get; } = new List<Vertex>();

    public TriangulatedQuadGrid(Rect bound, Vector2Int division, int seed) {
        PRNG = new PRNG(seed);
        ComputeGeometry(bound, division);
        ComputeNeighboring();

    }

    void ComputeGeometry(Rect bound, Vector2Int division) {
        var quadWidth = bound.width / division.x;
        var quadHeight = bound.height / division.y;
        for (int y = 0; y <= division.y; y++) {
            for (int x = 0; x <= division.x; x++) {
                Corners.Add(Vertex.New(new Vector2(bound.x + x * quadWidth, bound.y + y * quadHeight)));
            }
        }

        for (int yy = 0; yy < division.y; yy++) {
            for (int xx = 0; xx < division.x; xx++) {
                var a = (yy % (division.y + 1)) * (division.x + 1) + xx;
                var b = a + 1;
                var c = b + division.x + 1;
                var d = a + division.x + 1;
                Triangles.AddRange(RandomTrianglePair(Corners[a], Corners[b], Corners[c], Corners[d]));
            }
            //break;
        }
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

    List<Triangle> RandomTrianglePair(params Vertex[] corners) {
        var cornerList = corners.ToList();
        cornerList.Reorder();
        var triangles = new List<Triangle>();
        if (PRNG.Bool()) {
            triangles.Add(CreateTriangle(cornerList, 0, 1, 2));
            triangles.Add(CreateTriangle(cornerList, 0, 2, 3));
        }
        else {
            triangles.Add(CreateTriangle(cornerList, 1, 2, 3));
            triangles.Add(CreateTriangle(cornerList, 1, 3, 0));
        }
        return triangles;
    }

    Triangle CreateTriangle(List<Vertex> corners, params int[] indexes) {
        return new Triangle(new List<Vertex> { corners[indexes[0]], corners[indexes[1]], corners[indexes[2]] });
    }

    public List<Triangle> Triangles { get; } = new List<Triangle>();

    public Dictionary<Triangle, List<Triangle>> NeighborsByTriangle { get; } = new Dictionary<Triangle, List<Triangle>>();

    public HashSet<Pair<Triangle>> TriangleNeighborPairs { get; } = new HashSet<Pair<Triangle>>();
}
