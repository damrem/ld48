using System;
using UnityEngine;

namespace Damrem.Grids.Irregular {

    [Serializable]
    public class BaseTriangleGridHiveDef {
        public Rect Bound = new Rect(0, 0, 1, 1);
        public float CellRadius = 1;
        public int CellSubdivisionCount = 1;
    }
}