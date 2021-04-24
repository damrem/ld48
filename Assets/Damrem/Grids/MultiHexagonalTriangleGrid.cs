using Damrem.DataStructures;
using Damrem.Geom2D;
using Damrem.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Grids {

    public class MultiHexagonalTriangleGrid : ITriangleGrid {
        readonly List<HexagonalTriangleGrid> Hexagons = new List<HexagonalTriangleGrid>();

        public MultiHexagonalTriangleGrid(float hexRadius, int subdivision, Rect bounds) {
            var hexagonCenters = HexagonalGrid.CreatePoints(hexRadius, bounds);
            Hexagons = hexagonCenters.Map(c => new HexagonalTriangleGrid(c, hexRadius, subdivision));
            ComputeTriangles();
            ComputeNeighboring();
        }

        void ComputeTriangles() {
            Hexagons.ForEach((global::System.Action<HexagonalTriangleGrid>)(h => {
                this.Triangles.AddRange(h.Triangles);
            }));
        }

        void ComputeNeighboring() {
            ComputeIntraNeighboring();
            ComputeInterNeighboring();
        }

        void ComputeIntraNeighboring() {
            Hexagons.ForEach(h => {
                NeighborsByTriangle.AddRange(h.NeighborsByTriangle);
                TriangleNeighborPairs.UnionWith(h.TriangleNeighborPairs);
            });
        }

        void ComputeInterNeighboring() {
            foreach (var h in Hexagons) {
                foreach (var j in Hexagons.Exclude(h)) {
                    foreach (var ht in h.BoundTriangles) {
                        foreach (var jt in j.BoundTriangles) {
                            if (ht.IsAdjacentTo(jt)) {
                                NeighborsByTriangle.AddToKey(ht, jt);
                                NeighborsByTriangle.AddToKey(jt, ht);
                                TriangleNeighborPairs.Add(new Pair<Triangle>(ht, jt));
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<Triangle, List<Triangle>> NeighborsByTriangle { get; } = new Dictionary<Triangle, List<Triangle>>();

        public HashSet<Pair<Triangle>> TriangleNeighborPairs { get; } = new HashSet<Pair<Triangle>>();

        public List<Triangle> Triangles { get; } = new List<Triangle>();
    }
}
