using Damrem.System;
using Damrem.Collections;
using Damrem.UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Geom2D {
    [Serializable]
    public class Edge : IGeometricalElement, IEquatable<Edge> {
        public Vertex A;
        public Vertex B;

        public int HashCode { get { return GetHashCode(); } }

        public Edge(Vertex a, Vertex b) {
            A = a;
            B = b;
        }

        public Vertex Other(Vertex vertex) {
            if (vertex == A)
                return B;

            if (vertex == B)
                return A;

            return null;
        }

        public float Size { get { return Vector2.Distance(A.Position, B.Position); } }

        public bool HasVertex(Vertex corner) {
            return corner == A || corner == B;
        }

        public bool Equals(Edge other) {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return (A == other.A && B == other.B) || (A == other.B && B == other.A);
        }

        public Vector2 Centroid {
            get {
                return (A.Position + B.Position) / 2;
            }
        }

        public Vector2 Lerp(float t) {
            return Vector2.Lerp(A.Position, B.Position, t);
        }

        public override bool Equals(object obj) {
            return Equals(obj as Edge);
        }

        public static bool operator ==(Edge a, Edge b) {
            return a.Equals(b);
        }

        public static bool operator !=(Edge a, Edge b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            var corners = new List<Vertex>() { A, B };
            corners.Reorder();
            return HashCodeHelper.CombineHashCodes(corners[0], corners[1]);
        }

        public override string ToString() {
            var corners = new List<Vertex>() { A, B };
            corners.Reorder();
            return $"[Edge|{corners.Map(c => c.Position).GetCentroid()}]";
        }
    }
}
