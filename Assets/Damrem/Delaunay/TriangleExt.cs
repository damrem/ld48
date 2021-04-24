using csDelaunay;
using Damrem.UnityEngine;
using Damrem.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriangleExt {
    public static List<Vector2> GetCoords(this Triangle t) {
        return t.Sites.Map(s => s.Coord.ToVector2());
    }

    public static string ToString(this Triangle t) {
        return "[Triangle (" + t.GetCoords().AsString() + ")]";
    }
}
