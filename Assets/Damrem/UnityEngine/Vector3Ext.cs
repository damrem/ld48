using System.Collections.Generic;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class Vector3Ext {

        public static Vector2 ToVector2(this Vector3 v, ProjectionPlane plane = ProjectionPlane.XZ) {
            switch (plane) {
                case ProjectionPlane.XY: return new Vector2(v.x, v.y);

                default:
                case ProjectionPlane.XZ: return new Vector2(v.x, v.z);

                case ProjectionPlane.YZ: return new Vector2(v.y, v.z);
            }
        }

        public static void Scale(Vector3 from, Vector3 to, out Vector3 scaledFrom, out Vector3 scaledTo, float scale) {
            float actualScale = (1 + scale) / 2;
            scaledFrom = Vector3.LerpUnclamped(to, from, actualScale);
            scaledTo = Vector3.LerpUnclamped(from, to, actualScale);
        }

        public static Vector3 GetCentroid(this List<Vector3> points) {
            Vector3 centroid = Vector3.zero;
            foreach (var p in points)
                centroid += p;
            return centroid / points.Count;
        }

        public static Vector3 WithY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }
    }
}
