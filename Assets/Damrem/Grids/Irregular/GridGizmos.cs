using Damrem.Collections;
using Damrem.Geom2D;
using Damrem.Geom2D.Gizmos;
using Damrem.UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGizmos = UnityEngine.Gizmos;

namespace Damrem.Grids.Irregular {

    [RequireComponent(typeof(GridEntity))]
    public class GridGizmos : MonoBehaviour {

        public enum BorderGizmoType {
            None,
            Vertices,
            Edges,
            Quads,
        }

        [SerializeField] [Range(0, 1)] float VertexScale = .1f;
        [SerializeField] [Range(0, 1)] float EdgeScale = .75f;
        [SerializeField] [Range(0, 1)] float QuadScale = .5f;
        [SerializeField] [Range(0, 1)] float NeighboringScale = .25f;
        [SerializeField] bool ShouldDrawBaseTriangleGridGizmos = false;
        [SerializeField] BorderGizmoType BorderType = BorderGizmoType.None;

        [SerializeField] ITriangleGrid BaseTriangleGrid;

        IrregularQuadGrid GetGrid() {
            return GetComponent<GridEntity>().Grid;
        }

        ITriangleGrid GetBaseTriangleGrid() {
            if (BaseTriangleGrid is null) BaseTriangleGrid = GetGrid()?.BaseTriangleGrid;

            return BaseTriangleGrid;
        }

        void OnDrawGizmos() {
            if (ShouldDrawBaseTriangleGridGizmos) DrawBaseTriangleGridGizmos();
            if (QuadScale > 0) DrawQuads();
            if (EdgeScale > 0) DrawEdges();
            if (VertexScale > 0) DrawVertices();
            if (NeighboringScale > 0) DrawNeighborings(NeighboringScale);
            DrawBorders();
            //if(ShouldDrawFullGraphGizmos) DrawFullGraphGizmos();
        }

        void DrawNeighborings(float scale) {
            var grid = GetGrid();
            if (grid?.Neighborings is null) return;

            foreach (var neighboring in grid.Neighborings) {
                if (neighboring.A is null || neighboring.B is null) continue;

                var from = neighboring.A.GetCentroid().ToVector3();
                var to = neighboring.B.GetCentroid().ToVector3();
                GizmosExt.DrawScaledDottedLine(from, to, scale, .01f);
            }
        }

        void DrawEdges() {
            DrawEdges(GetGrid()?.Edges.ToList(), EdgeScale);
        }

        void DrawEdges(List<Edge> edges, float scale) {
            if (edges is null) return;

            edges.ForEach(e => {
                e.DrawGizmo(scale);
            });
        }

        void DrawPathsGizmos(List<List<Quadrangle>> paths) {
            if (paths is null) return;

            foreach (var path in paths)
                DrawPathsGizmo(path);
        }

        void DrawPathsGizmos(HashSet<Quadrangle> pathQuads) {
            if (pathQuads is null) return;

            foreach (var q in pathQuads)
                q.DrawGizmo();
        }

        void DrawPathsGizmo(List<Quadrangle> path) {
            foreach (var q in path) {
                q.DrawGizmo();
            }
        }

        void DrawBaseTriangleGridGizmos() {
            var baseTriangleGrid = GetBaseTriangleGrid();
            if (baseTriangleGrid is null) return;

            var grid = baseTriangleGrid as TriangulatedQuadGrid;

            foreach (var t in grid.Triangles) {
                t.DrawGizmo(.875f);
            }
            // for(int i = 0 ; i < grid.Corners.Count ; i++) {
            //     var c = grid.Corners[i];
            //     UnityGizmos.DrawCube(c.ToVector3(), Vector3.one / 2);
            //     //GizmosExt.DrawString(i.ToString(), c.ToVector3(.5f));
            // }
        }

        void DrawQuads() {
            DrawQuads(GetGrid()?.Quadrangles?.ToList(), QuadScale);
        }

        void DrawQuads(List<Quadrangle> quads, float scale) {
            quads?.DrawGizmos(scale);
        }

        void DrawVertices() {
            DrawVertices(GetGrid()?.Vertices, VertexScale);
        }

        void DrawVertices(HashSet<Vertex> vertices, float scale) {
            if (vertices is null) return;

            foreach (var c in vertices) {
                UnityGizmos.DrawCube(c.ToVector3(), Vector3.one * scale);
            }
        }

        void DrawBorders() {
            var grid = GetGrid();

            if (grid is null)
                return;

            switch (BorderType) {
                default: case BorderGizmoType.None: return;
                case BorderGizmoType.Vertices: DrawVertices(grid.BorderVertices, 1); break;
                case BorderGizmoType.Edges: DrawEdges(grid.BorderEdges, 1); break;
                case BorderGizmoType.Quads: DrawQuads(grid.BorderQuads, 1); break;
            }
        }

        //void DrawFullGraphGizmos() {
        //    if(FullTileGraph is null) return;
        //    if(FullTileGraph.Graph is null) return;

        //    foreach(var edge in FullTileGraph.Graph.Edges) {
        //        GizmosExt.DrawDottedLine(edge.Source.Centroid3D(true), edge.Destination.Centroid3D(true));
        //    }
        //}
    }
}