using System;

namespace Damrem.Grids.Irregular {

    [Serializable]
    public class GenerationDef {
        public int Seed = 0;
        public BaseTriangleGridType BaseTriangleGridType = BaseTriangleGridType.Hive;
    }
}