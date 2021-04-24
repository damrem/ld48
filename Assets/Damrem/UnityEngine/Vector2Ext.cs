using Damrem.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class Vector2Ext {

        public static readonly Pool<Vector2, float, float> Pool = new Pool<Vector2, float, float>(
        (x, y) => new Vector2(x, y),
        (v, x, y) => { v.x = x; v.y = y; },
        v => { v.x = 0; v.y = 0; }
    );

        public static Vector2 UnitVector(this Vector2 vector) {
            Vector2 unitVector = new Vector2();

            float dx = Mathf.Abs(vector.x);
            float dy = Mathf.Abs(vector.y);

            if (dx > dy) {
                unitVector.x = vector.x / dx;
                unitVector.y = 0;
            }
            else {
                unitVector.x = 0;
                unitVector.y = vector.y / dy;
            }

            return unitVector;
        }

        public static Vector2 Rotate(this Vector2 vector, float degrees) {
            return Quaternion.Euler(0, 0, degrees) * vector;
        }

        public static Vector2 Clone(this Vector2 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2 GetNormal(this Vector2 vector) {
            return vector.Rotate(90);
        }

        public static Vector2 Normalized(this Vector2 vector, float length = 1.0f) {
            Vector2 clone = vector.Clone();
            clone.Scale(new Vector2(length, length));
            return clone;
        }

        public static Vector2f ToVector2f(this Vector2 v2) {
            return new Vector2f(v2.x, v2.y);
        }

        public static Dictionary<Vector2f, T> ToVector2fKeyedDictionary<T>(this Dictionary<Vector2, T> dictionary) {
            Dictionary<Vector2f, T> v2fKeyedDictionary = new Dictionary<Vector2f, T>();
            foreach (KeyValuePair<Vector2, T> pair in dictionary) {
                v2fKeyedDictionary.Add(pair.Key.ToVector2f(), pair.Value);
            }
            return v2fKeyedDictionary;
        }

        public static float DistanceSquare(this Vector2 a, Vector2 b) {
            float cx = b.x - a.x;
            float cy = b.y - a.y;
            return cx * cx + cy * cy;
        }

        public static Vector3 ToVector3(this Vector2 v, float missing, bool topDown = true) {
            float x = v.x;
            float y = v.y;
            float z = missing;
            if (topDown)
                return new Vector3(x, z, y);
            else
                return new Vector3(x, y, z);
        }

        public static Vector3 ToVector3(this Vector2 v, ProjectionPlane projectionPlane = ProjectionPlane.XZ) {
            switch (projectionPlane) {
                case ProjectionPlane.XY: return new Vector3(v.x, v.y, 0);
                default:
                case ProjectionPlane.XZ: return new Vector3(v.x, 0, v.y);
                case ProjectionPlane.YZ: return new Vector3(0, v.x, v.y);
            }
        }

        public static Vector2 GetCentroid(this List<Vector2> points) {
            Vector2 centroid = Vector2.zero;
            foreach (var p in points)
                centroid += p;
            return centroid / points.Count;
        }

        public static void Reorder(this List<Vector2> points) {
            Vector2 barycenter = GetCentroid(points);
            points.Sort((a, b) => {
                var localA = a - barycenter;
                var angleA = PositiveAngle(localA);
                var localB = b - barycenter;
                var angleB = PositiveAngle(localB);
                if (angleA == angleB)
                    return 0;
                var delta = angleA - angleB;
                if (delta > 0)
                    return 1;
                return -1;
            });
        }

        public static float PositiveAngle(Vector2 point) {
            return ClampedAngle(point, Vector2.right);
        }

        public static float ClampedAngle(Vector2 from, Vector2 to) {
            var signed = Vector2.SignedAngle(from, to);
            while (signed < 0)
                signed += 360;
            return signed;
        }

        public static float ClampedAngle(this Vector2 point) {
            return ClampedAngle(Vector2.right, point);
        }

        public static void Round(this Vector2 v, float precision = 1 / 1000) {
            v.x = MathfExt.Round(v.x, precision);
            v.y = MathfExt.Round(v.y, precision);
        }

        public static Vector2Int ToVector2Int(this Vector2 v) {
            return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.x));
        }

        public static void Clean(this Vector2 v) {
            v.x = 0;
            v.y = 0;
        }
    }
}

