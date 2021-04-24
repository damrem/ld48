using Damrem.Collections;
using Damrem.UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityGizmos = UnityEngine.Gizmos;

namespace Damrem.Geom2D.Gizmos {

    public static class Geom2DGizmos {
        public static void DrawGizmo(this Quadrangle quadrangle, float scale = 1, float y = 0) {
            var centroid = quadrangle.GetCentroid().ToVector3(y);
            quadrangle.Vertices.ForEach((p, i) => {
                var a = Vector3.LerpUnclamped(centroid, p.ToVector3(y), scale);
                int nextI = (i + 1) % quadrangle.Vertices.Count;
                var b = Vector3.LerpUnclamped(centroid, quadrangle.Vertices[nextI].ToVector3(y), scale);
                UnityGizmos.DrawLine(a, b);
            });
        }

        public static void DrawGizmos(this List<Quadrangle> quadrangles, float scale = 1, float y = 0) {
            if (quadrangles == null)
                return;

            foreach (var q in quadrangles)
                DrawGizmo(q, scale, y);
        }

        public static void DrawGizmos(this HashSet<Quadrangle> quadrangles, float scale = 1, float y = 0) {
            if (quadrangles == null)
                return;

            foreach (var q in quadrangles)
                DrawGizmo(q, scale, y);
        }

        public static void DrawGizmo(this Polygon p, float scale = 1) {
            var centroid = p.GetCentroid();
            var vertices = p.Vertices.Map(v => Vector2.Lerp(centroid, v.Position, scale)).Map(Vertex.New);
            DrawGizmo(vertices);
        }

        public static void DrawGizmo(this List<Vertex> corners) {
            for (int i = 0; i < corners.Count; i++) {
                GizmosExt.DrawDottedLine(corners[i].ToVector3(), corners[(i + 1) % corners.Count].ToVector3());
            }
        }

        public static void DrawGizmos(this List<Polygon> polygons) {
            foreach (var p in polygons) DrawGizmo(p);
        }

        public static void DrawTriangles(this List<Triangle> triangles, float scale = 1) {
            foreach (var t in triangles)
                DrawTriangle(t, scale);

        }

        public static void DrawTriangle(this Triangle triangle, float scale) {
            var barycenter = triangle.GetCentroid().ToVector3();
            for (int i = 0; i < triangle.Vertices.Count; i++) {
                int nextI = (i + 1) % triangle.Vertices.Count;
                var from = Vector3.LerpUnclamped(barycenter, triangle.Vertices[i].ToVector3(), scale);
                var to = Vector3.LerpUnclamped(barycenter, triangle.Vertices[nextI].ToVector3(), scale);
                UnityGizmos.DrawLine(from, to);
            }
        }

        public static void DrawGizmo(this Edge edge, float scale = 1) {
            GizmosExt.DrawScaledLine(edge.A.Position.ToVector3(), edge.B.Position.ToVector3(), scale);
        }

    }
}
