using csDelaunay;

public static class EdgeExt {
    public static Vector2f LeftEnd(this Edge edge) {
        if (edge == null) return Vector2f.zero;
        if (edge.ClippedEnds == null) return Vector2f.zero;
        return edge.ClippedEnds[LR.LEFT];
    }

    public static Vector2f RightEnd(this Edge edge) {
        if (edge == null) return Vector2f.zero;
        if (edge.ClippedEnds == null) return Vector2f.zero;
        return edge.ClippedEnds[LR.RIGHT];
    }
}