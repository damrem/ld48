using System;
using UnityEngine;

namespace Damrem.Grids.Irregular {

    [Serializable]
    public class BaseTriangleGridRectangularDef {
        public Rect Bound = new Rect(0, 0, 1, 1);
        public Vector2Int Subdivision = Vector2Int.one;
    }
}