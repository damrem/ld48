using System;

namespace Damrem.Grids.Irregular {

    [Serializable]
    public class RelaxationDef {
        public RelaxationType Type = RelaxationType.AverageDistanceToQuadCentroid;
        public int IterationCount = 10;
        public float Strength = .25f;
        public bool FreezeBorders = false;
    }
}