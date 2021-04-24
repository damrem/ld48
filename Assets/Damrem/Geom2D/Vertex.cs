using Damrem.Pooling;
using Damrem.System;
using Damrem.Collections;
using Damrem.UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Damrem.Geom2D {

    public class Vertex : IEquatable<Vertex>, ISerializable {
        public readonly static Pool<Vertex, Vector2> Pool = new Pool<Vertex, Vector2>(
            v => new Vertex(Vector2Ext.Pool.TakeObject(v.x, v.y)),
            (c, v) => { c.Position = v; }
        );

        public static Vertex New(Vector2 v) {
            return Pool.TakeObject(v);
        }

        public static int HASH_CODE_PRECISION = 3;

        public Vector2 Position;

        public static bool VeryClose(Vertex a, Vertex b) {
            return Distance(a, b) < .001;
        }

        public float X {
            get { return Position.x; }
            set { Position = new Vector2(value, Position.y); }
        }
        public float Y {
            get { return Position.y; }
            set { Position = new Vector2(Position.x, value); }
        }

        Vertex(Vector2 position) {
            Position = position;
        }

        public static float Distance(Vertex a, Vertex b) {
            return Vector2.Distance(a.Position, b.Position);
        }

        public static float Distance(Vertex a, Vector2 b) {
            return Vector2.Distance(a.Position, b);
        }

        public static float Distance(Vector2 a, Vertex b) {
            return Vector2.Distance(a, b.Position);
        }

        public bool Equals(Vertex other) {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return VeryClose(this, other);
        }

        public override bool Equals(object obj) {
            return Equals(obj as Vertex);
        }

        public static bool operator ==(Vertex a, Vertex b) {
            return a.Equals(b);
        }

        public static bool operator !=(Vertex a, Vertex b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            return HashCodeHelper.CombineHashCodes(Math.Round(Position.x, HASH_CODE_PRECISION), Math.Round(Position.y, HASH_CODE_PRECISION));
        }

        public override string ToString() {
            return $"Vertex{Position}";
        }

        public Vector3 ToVector3(float y = 0) {
            return Position.ToVector3(y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("X", X, typeof(float));
            info.AddValue("Y", Y, typeof(float));
        }
    }

    public static class VertexExt {
        public static Vector2 GetCentroid(this List<Vertex> vertices) {
            return vertices.Map(v => v.Position).GetCentroid();
        }

        public static void Reorder(this List<Vertex> vertices) {
            vertices.Reorder(vertices.GetCentroid());
        }

        public static void Reorder(this List<Vertex> vertices, Vector2 pivot) {
            vertices.Sort((a, b) => {
                var localA = a.Position - pivot;
                var angleA = Vector2Ext.PositiveAngle(localA);
                var localB = b.Position - pivot;
                var angleB = Vector2Ext.PositiveAngle(localB);
                if (angleA == angleB)
                    return 0;
                var delta = angleA - angleB;
                if (delta > 0)
                    return 1;
                return -1;
            });
        }

        public static int GetHashCode(this List<Vertex> vertices) {
            var orderedVertices = new List<Vertex>(vertices);
            orderedVertices.Reorder();
            return HashCodeHelper.CombineHashCodes(orderedVertices.Map(c => c.GetHashCode()));
        }

        public static void Clean(this List<Vertex> vertices) {
            foreach (var v in vertices)
                Vertex.Pool.PutObject(v);

            vertices.Clear();
        }


    }
}
