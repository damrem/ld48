using Damrem.System;
using Damrem.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Geom2D {

    public class Polygon : IGeometricalElement, IEquatable<Polygon>, IComparable<Polygon> {

        public List<Vertex> Vertices { get; internal set; }

        public Polygon() {
            Vertices = new List<Vertex>();
        }

        Vector2 Centroid;
        bool CentroidHasBeenComputed = false;
        public Vector2 GetCentroid() {
            if (!CentroidHasBeenComputed) {
                Centroid = Vertices.GetCentroid();
                CentroidHasBeenComputed = true;
            }
            return Centroid;

        }

        void Construct(List<Vertex> corners, int cornerCount = -1) {
            Vertices = new List<Vertex>(new HashSet<Vertex>(corners));
            if (cornerCount > -1 && Vertices.Count != cornerCount)
                throw new InvalidVertexCountException();
            Vertices.Reorder();
        }

        public Polygon(List<Vertex> corners) {
            Construct(corners);
        }

        public Polygon(List<Vertex> corners, int cornerCount) {
            Construct(corners, cornerCount);
        }

        public List<Edge> Edges {
            get {
                return Vertices.Map((corner, i) => {
                    var nextI = (i + 1) % Vertices.Count;
                    return new Edge(Vertices[i], Vertices[nextI]);
                });
            }
        }

        public bool HasCorner(Vertex corner) {
            return Vertices.Contains(corner);
        }

        public int CornerDistance(Vertex A, Vertex B) {
            if (!HasCorner(A) || !HasCorner(B))
                return -1;

            return Mathf.Abs(Vertices.IndexOf(A) - Vertices.IndexOf(B)) % Mathf.FloorToInt(Vertices.Count / 2);
        }

        static bool CompareCorners(Polygon a, Polygon b) {
            var allCorners = new HashSet<Vertex>();
            allCorners.UnionWith(a.Vertices);
            allCorners.UnionWith(b.Vertices);
            return allCorners.Count == a.Vertices.Count;
        }

        public bool Equals(Polygon other) {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return CompareCorners(this, other);
        }

        public override int GetHashCode() {
            return HashCodeHelper.CombineHashCodes(Vertices);
        }

        public override bool Equals(object obj) {
            return Equals(obj as Polygon);
        }

        public static bool operator ==(Polygon a, Polygon b) {
            if (a is null) {
                if (b is null)
                    return true;
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Polygon a, Polygon b) {
            return !(a == b);
            ;
        }

        public bool IsAdjacentTo(Polygon other) {
            if (this == other)
                return false;

            var corners = new HashSet<Vertex>(Vertices);
            corners.UnionWith(other.Vertices);
            return corners.Count == Vertices.Count + other.Vertices.Count - 2;
        }

        override public string ToString() {
            return ToString("Polygon");
        }

        public string ToString(string name) {
            return $"[{name}{GetCentroid()}]";
        }

        readonly float HalfWay = .5f;
        readonly List<Vertex> SubCorners = new List<Vertex>();
        int SubCornerIndex;
        readonly List<Vertex> MidPoints = new List<Vertex>();
        readonly List<Vertex> SubQuadrangleCorners = new List<Vertex>();
        public List<Quadrangle> Quadrangulate() {
            Vertices.Reorder();

            SubCorners.Clear();
            SubCorners.AddRange(Vertices);

            MidPoints.Clear();
            MidPoints.AddRange(Vertices.Map((corner, i) => {
                return Vertex.Pool.TakeObject(Vector2.Lerp(
                    Vertices[i].Position,
                    Vertices[(i + 1) % Vertices.Count].Position,
                    HalfWay
                ));
            }));
            SubCorners.AddRange(MidPoints);
            SubCorners.Reorder();

            return Vertices.Map(corner => {
                SubCornerIndex = SubCorners.FindIndex(corner.Equals);
                SubQuadrangleCorners.Clear();
                SubQuadrangleCorners.Add(corner);
                SubQuadrangleCorners.Add(SubCorners[(SubCornerIndex + 1) % SubCorners.Count]);
                SubQuadrangleCorners.Add(Vertex.Pool.TakeObject(GetCentroid()));
                SubQuadrangleCorners.Add(SubCorners[(SubCornerIndex - 1 + SubCorners.Count) % SubCorners.Count]);
                return Quadrangle.New(SubQuadrangleCorners);
            });
        }

        public int CompareTo(Polygon other) {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            if (GetHashCode() > other.GetHashCode()) return 1;
            return -1;
        }

        public Polygon GetScaled(float scale = 1) {
            var centroid = GetCentroid();
            var corners = Vertices.Map((c, i) => {
                var v = Vector2.LerpUnclamped(centroid, c.Position, scale);
                return Vertex.New(v);
            });
            return new Polygon(corners);
        }

    }
}
