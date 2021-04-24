using System.Collections.Generic;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class Vector2fExt {
        public static Vector2f UnitVector(this Vector2f vector) {
            Vector2f unitVector = new Vector2f();

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

        public static Vector3 ToVector3(this Vector2f vector2f, bool xy = false, float missing = 0) {
            return new Vector3(vector2f.x, xy ? vector2f.y : missing, xy ? missing : vector2f.y);
        }

        public static Vector2 ToVector2(this Vector2f v2f) {
            return new Vector2(v2f.x, v2f.y);
        }

        public static Dictionary<Vector2, T> ToVector2KeyedDictionary<T>(this Dictionary<Vector2f, T> dictionary) {
            Dictionary<Vector2, T> v2KeyedDictionary = new Dictionary<Vector2, T>();
            foreach (KeyValuePair<Vector2f, T> pair in dictionary) {
                v2KeyedDictionary.Add(pair.Key.ToVector2(), pair.Value);
            }
            return v2KeyedDictionary;
        }
    }
}

