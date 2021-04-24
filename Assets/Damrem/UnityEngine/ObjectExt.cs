using ObSolete = System.ObsoleteAttribute;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class ObjectExt {
        [ObSolete("Use ObjectExt.Destroy() instead.")]
        public static void Destroyish(this Object obj) {
            Destroy(obj);
        }

        public static void Destroy(this Object obj) {
            if (Application.isPlaying) Object.Destroy(obj);
            else Object.DestroyImmediate(obj);
        }
    }
}