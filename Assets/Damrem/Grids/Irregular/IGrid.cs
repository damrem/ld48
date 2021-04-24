using System.Collections.Generic;
using Damrem.Geom2D;

namespace Damrem.Grids.Irregular {
    interface IGrid {
        List<Vertex> Corners { get; set; }
        List<Quadrangle> Quadrangles { get; set; }
    }
}