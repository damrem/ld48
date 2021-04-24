using UnityEngine;

public static class RectfExt {
    public static Rect ToRect(this Rectf rectf) {
        return new Rect(rectf.x, rectf.y, rectf.width, rectf.height);
    }
}
