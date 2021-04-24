using Damrem.Geom2D;
using Damrem.Collections;
using Damrem.UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Damrem.Grids.Irregular {

    public enum RelaxationType {
        None,
        ToAdjacentVerticesCentroid,
        AverageDistanceToQuadCentroid,
        OskStaStyle,
        SideSize,
    }

    public static class RelaxationStrategies {

        /**
         *  This relaxation strategy does not affect borders. 
         *  Got to figure out why it does not event move corners along the borders.
         **/
        public static void RelaxToAdjacentCornersBarycenter(this IrregularQuadGrid grid, float factor) {
            grid.ComputeGeometry();

            var cornerMap = new Dictionary<Vertex, Vertex>();

            foreach (var corner in grid.Vertices) {
                if (grid.BorderVertices.Contains(corner)) {
                    cornerMap.Add(corner, corner);
                    continue;
                }
                var sides = grid.EdgesByVertex[corner];
                var otherCorners = sides.ToList().Map(side => side.Other(corner));
                var barycenter = otherCorners.GetCentroid();
                var newCorner = Vertex.Pool.TakeObject(Vector2.Lerp(corner.Position, barycenter, factor));
                cornerMap.Add(corner, newCorner);
            }

            grid.Quadrangles.Replace(grid.Quadrangles.Map(q => Quadrangle.New(q.Vertices.Map(c => cornerMap[c]))));

        }

        //TODO RelaxToAdjacentSidesBarycenterBarycenter
        //TODO RelaxToAdjacentQuadranglesBarycenterBarycenter
        static Dictionary<Vertex, Vector2> CreateForcesByCorner(HashSet<Vertex> corners) {
            var forcesByCorner = new Dictionary<Vertex, Vector2>();
            foreach (var c in corners)
                forcesByCorner[c] = new Vector2();
            return forcesByCorner;
        }

        public static void RelaxAverageDistanceToQuadrangleBarycenter(this IrregularQuadGrid grid, float factor, bool shouldFreezeBorders) {
            grid.ComputeGeometry();

            var cornerMap = new Dictionary<Vertex, Vertex>();
            var forcesByCorner = CreateForcesByCorner(grid.Vertices);

            float avgDistToCenter = 0;
            foreach (var q in grid.Quadrangles) {
                var centroid = q.GetCentroid();
                foreach (var c in q.Vertices) {
                    avgDistToCenter += Vector2.Distance(c.Position, centroid);
                }
            }
            avgDistToCenter /= grid.Quadrangles.Count * 4;

            foreach (var q in grid.Quadrangles) {
                var centroid = q.GetCentroid();
                var movingCorners = q.Vertices;

                if (shouldFreezeBorders)
                    movingCorners = movingCorners.FindAll(c => !grid.BorderVertices.Contains(c));

                foreach (var c in movingCorners) {
                    var localPos = c.Position - centroid;
                    var currentDist = localPos.magnitude;
                    var ratio = avgDistToCenter / currentDist;
                    var destPos = localPos.normalized * avgDistToCenter;
                    var force = (destPos - localPos) * factor;
                    forcesByCorner[c] += force;
                }
            }

            foreach (var forceByCorner in forcesByCorner) {
                var c = forceByCorner.Key;
                var f = forceByCorner.Value;
                var v = new Vector2(c.X + f.x, c.Y + f.y);
                var newCorner = Vertex.Pool.TakeObject(v);
                cornerMap.Add(c, newCorner);
            }

            grid.Quadrangles.Replace(grid.Quadrangles.Map(q => Quadrangle.New(q.Vertices.Map(c => cornerMap[c]))));
        }

        public static void RelaxSideSize(this IrregularQuadGrid grid, float factor, bool shouldFreezeBorders) {
            grid.ComputeGeometry();

            var cornerMap = new Dictionary<Vertex, Vertex>();
            var forcesByCorner = CreateForcesByCorner(grid.Vertices);

            var sideAvgSize = grid.Edges.Map(side => side.Size).Average();

            foreach (var q in grid.Quadrangles) {
                foreach (var s in q.Edges) {
                    var sizeDelta = sideAvgSize - s.Size;
                    var forceA = (s.A.Position - s.B.Position).normalized * sizeDelta / 2 * factor;
                    forcesByCorner[s.A] += forceA;
                    forcesByCorner[s.B] -= forceA;
                }
            }

            foreach (var forceByCorner in forcesByCorner) {
                var c = forceByCorner.Key;
                var f = forceByCorner.Value;
                if (!shouldFreezeBorders || !grid.BorderVertices.Contains(c)) {
                    var v = new Vector2(c.X + f.x, c.Y + f.y);
                    var newCorner = Vertex.Pool.TakeObject(v);
                    cornerMap.Add(c, newCorner);
                }
                else
                    cornerMap.Add(c, c);
            }

            grid.Quadrangles.Replace(grid.Quadrangles.Map(q => Quadrangle.New(q.Vertices.Map(c => cornerMap[c]))));
        }

        public static void RelaxOskSta(this IrregularQuadGrid grid, float factor, bool shouldFreezeBorders) {
            grid.ComputeGeometry();
            var forcesByCorner = new Dictionary<Vertex, Vector2>();
            foreach (var q in grid.Quadrangles) {
                foreach (var c in q.Vertices)
                    forcesByCorner[c] = new Vector2();
            }
            //var forcesByCorner = CreateForcesByCorner(grid.Corners);  //  corners key issue :(
            foreach (var q in grid.Quadrangles) {
                var force = new Vector2();
                var centroid = q.GetCentroid();
                foreach (var c in q.Vertices) {
                    force += c.Position - centroid;
                    force = new Vector2(force.y, -force.x);
                }
                force /= q.Vertices.Count;
                foreach (var c in q.Vertices) {
                    forcesByCorner[c] += centroid + force - c.Position;
                    force = new Vector2(force.y, -force.x);
                }
            }
            var relaxeds = new HashSet<Quadrangle>();
            foreach (var q in grid.Quadrangles) {
                var relaxedsCorners = q.Vertices.Map(c => {
                    var pos = c.Position;
                    var force = forcesByCorner[c] * factor;
                    if (shouldFreezeBorders && grid.BorderVertices.Contains(c))
                        force = new Vector2();

                    var v = new Vector2(pos.x + force.x, pos.y + force.y);
                    var newCorner = Vertex.Pool.TakeObject(v);
                    return newCorner;
                });
                relaxeds.Add(Quadrangle.New(relaxedsCorners));
            }
            grid.Quadrangles.Replace(relaxeds);
        }
    }

}