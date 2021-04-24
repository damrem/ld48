using UnityEngine;

namespace Damrem.UnityEngine {

    public static class ColorExt {
        public static Color transparent = new Color(0, 0, 0, 0);

        public static Color Alpha(this Color color, float alpha) {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}