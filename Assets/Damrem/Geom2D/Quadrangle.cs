using Damrem.Pooling;
using Damrem.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Geom2D {

    [Serializable]
    public class Quadrangle : Polygon {

        Quadrangle(List<Vertex> corners) : base(corners, 4) { }

        public static Pool<Quadrangle, List<Vertex>> Pool = new Pool<Quadrangle, List<Vertex>>(
            corners => new Quadrangle(corners),
            (q, corners) => {
                q.Vertices = corners;
            }
        );

        public static Quadrangle New(List<Vertex> corners) {
            return Pool.TakeObject(corners);
        }

        override public string ToString() {
            return ToString("Quadrangle");
        }

        public static Edge GetCommonSide(Quadrangle a, Quadrangle b) {
            return a.Edges.Find(b.Edges.Contains);
        }

        public Edge GetOppositeSide(Edge side) {
            if (!Edges.Contains(side)) throw new Exception("Side is not part of this quadrangle.");

            return Edges.Find(s => !s.HasVertex(side.A) && !s.HasVertex(side.B));
        }

        public Edge GetLeftSide(Edge side) {
            var index = Edges.IndexOf(side) - 1;
            if (index < 0) index += Edges.Count;
            return Edges[index];
        }

        public Edge GetRightSide(Edge side) {
            var index = (Edges.IndexOf(side) + 1) % Edges.Count;
            return Edges[index];
        }

        public Edge GetSideTowards(Vector2 towards) {
            var quadCentroid = GetCentroid();
            return Edges.MinBy(s => Vector2.Distance(towards.normalized, (s.Centroid - quadCentroid).normalized));
        }

        public bool IsRectangular(float toleranceDeg = 0) {
            var a = Vertices[0].Position;
            var b = Vertices[1].Position;
            var c = Vertices[2].Position;
            var d = Vertices[3].Position;
            if (Mathf.Abs(90 - Vector2.Angle(a - b, a - d)) > toleranceDeg) return false;
            if (Mathf.Abs(90 - Vector2.Angle(c - b, c - d)) > toleranceDeg) return false;
            return true;
        }
    }

}
