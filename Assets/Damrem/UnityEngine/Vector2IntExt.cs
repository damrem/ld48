using UnityEngine;

namespace Damrem.UnityEngine {
    public static class Vector2IntExt {
        public static Vector3 ToVector3(this Vector2Int v, ProjectionPlane projection = ProjectionPlane.XZ) {
            switch (projection) {
                case ProjectionPlane.XY:
                    return new Vector3(v.x, v.y);

                default:
                case ProjectionPlane.XZ:
                    return new Vector3(v.x, 0, v.y);

                case ProjectionPlane.YZ:
                    return new Vector3(0, v.x, v.y);
            }
        }

        public static Vector2 ToVector2(this Vector2Int v) {
            return new Vector2(v.x, v.y);
        }
    }
}