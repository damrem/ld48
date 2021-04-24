using UnityEngine;

public static class RectExt {
    public static Rectf ToRectf(this Rect rect) {
        return new Rectf(rect.x, rect.y, rect.width, rect.height);
    }
}
