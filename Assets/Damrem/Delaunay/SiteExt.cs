using csDelaunay;
using Damrem.Collections;
using Damrem.UnityEngine;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Delaunay {
    static public class SiteExt {
        static public bool IsPartOfHull(this Site site, Rectf plotBounds) {
            List<Vector2f> region = site.Region(plotBounds);
            return region.Exists(point => point.x <= plotBounds.left || point.x >= plotBounds.right || point.y <= plotBounds.top || point.y >= plotBounds.bottom);
        }

        static public IEnumerable<Vertex> GetVertices(this Site site) {
            var vertices = site.Edges.Map(edge => new List<Vertex>() { edge.LeftVertex, edge.RightVertex }).GetFlattened();
            return new HashSet<Vertex>(vertices);
        }

        static public List<Vector2> Region(this Site site, Rect clippingBounds) {
            return site.Region(clippingBounds.ToRectf()).Map(p => p.ToVector2());
        }

        static public Vector2 GetCoord(this Site site) {
            return site.Coord.ToVector2();
        }
    }
}