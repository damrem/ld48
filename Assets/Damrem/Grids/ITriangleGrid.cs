using Damrem.DataStructures;
using Damrem.Geom2D;
using System.Collections.Generic;

namespace Damrem.Grids {
    public interface ITriangleGrid {
        List<Triangle> Triangles { get; }
        Dictionary<Triangle, List<Triangle>> NeighborsByTriangle { get; }
        HashSet<Pair<Triangle>> TriangleNeighborPairs { get; }
    }
}