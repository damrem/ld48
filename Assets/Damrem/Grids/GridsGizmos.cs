using Damrem.Geom2D.Gizmos;
using Damrem.UnityEngine;

namespace Damrem.Grids.Gizmo {

    public static class GridsGizmos {
        public static void DrawHexTriGrid(this ITriangleGrid hexTriGrid) {
            if (hexTriGrid == null) return;
            if (hexTriGrid.Triangles == null) return;
            foreach (var triangle in hexTriGrid.Triangles) {
                Geom2DGizmos.DrawTriangle(triangle, .85f);
            }

            foreach (var corners in ((HexagonalTriangleGrid)hexTriGrid).CornersByLevel) {
                Geom2DGizmos.DrawGizmo(corners);
            }
        }

        public static void DrawTriangleNeighboring(this ITriangleGrid triangleGrid, float y = 0) {
            if (triangleGrid is null)
                return;

            var triangleNeighborPairs = triangleGrid.TriangleNeighborPairs;

            if (triangleNeighborPairs is null)
                return;

            foreach (var pair in triangleNeighborPairs) {
                GizmosExt.DrawDottedLine(pair.A.GetCentroid().ToVector3(y), pair.B.GetCentroid().ToVector3(y), .1f);
            }
        }
    }
}