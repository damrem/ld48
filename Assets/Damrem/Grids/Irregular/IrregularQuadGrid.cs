using Damrem.DataStructures;
using Damrem.Geom2D;
using Damrem.Procedural;
using Damrem.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Damrem.Grids.Irregular {

    public class IrregularQuadGrid {
        public ITriangleGrid BaseTriangleGrid { get; private set; }
        public HashSet<Vertex> BorderVertices { get; private set; } = new HashSet<Vertex>();
        public List<Quadrangle> BorderQuads { get; private set; } = new List<Quadrangle>();
        public List<Edge> BorderEdges { get; } = new List<Edge>();
        public HashSet<Vertex> Vertices { get; set; } = new HashSet<Vertex>();
        public HashSet<Pair<Quadrangle>> Neighborings { get; } = new HashSet<Pair<Quadrangle>>();
        public Dictionary<Quadrangle, List<Quadrangle>> NeighborsByQuadrangle { get; } = new Dictionary<Quadrangle, List<Quadrangle>>();
        public HashSet<Quadrangle> Quadrangles { get; set; }
        public Dictionary<Vertex, List<Quadrangle>> QuadranglesByVertex { get; } = new Dictionary<Vertex, List<Quadrangle>>();
        public Dictionary<Edge, List<Quadrangle>> QuadranglesByEdge { get; } = new Dictionary<Edge, List<Quadrangle>>();
        public HashSet<Edge> Edges { get; set; } = new HashSet<Edge>();
        public Dictionary<Vertex, HashSet<Edge>> EdgesByVertex { get; } = new Dictionary<Vertex, HashSet<Edge>>();

        readonly PRNG Prng;

        public float AverageEdgeSize { get; private set; } = 1;
        readonly Object ProfilingTarget;
        List<Triangle> QuadrangulationComputedTriangles;
        List<Vertex> QuadrangulationVertices;

        public IrregularQuadGrid(HashSet<Vertex> vertices, HashSet<Quadrangle> quads, HashSet<Edge> edges) {
            Vertices = vertices;
            Quadrangles = quads;
            Edges = edges;
            ComputeGeometryAndNeighboring();
            ComputeBorders();
        }

        public IrregularQuadGrid(ITriangleGrid triangleGrid, int seed, Object profilingTarget, int subquadrangulationIterationCount = 1) {
            BaseTriangleGrid = triangleGrid;

            Prng = new PRNG(seed);

            ProfilingTarget = profilingTarget;

            var polygons = QuadrangulateTriangleGrid(triangleGrid/*, out List<Triangle> orphanTriangles*/);
            Quadrangles = SubQuadrangulate(polygons);

            SubQuadrangulate(subquadrangulationIterationCount - 1);

            ComputeGeometry();
            //ComputeNeighboring();
            ComputeBorders();
        }

        internal Quadrangle ClosestQuad(Vector2 vector2) {
            var quads = new List<Quadrangle>(Quadrangles);
            return quads.MinBy((q => Vector2.Distance(q.GetCentroid(), vector2)));
        }

        public void Clear(bool totally = false) {
            ClearBorders();
            ClearNeighboring();
            ClearGeometry(totally);
        }

        HashSet<Polygon> QuadrangulateTriangleGrid(ITriangleGrid triangleGrid/*, out List<Triangle> orphanTriangles*/) {
            Profiler.BeginSample("QuadrangulateTriangleGrid", ProfilingTarget);

            var polygons = new HashSet<Polygon>();
            QuadrangulationComputedTriangles = new List<Triangle>();
            foreach (var t in triangleGrid.Triangles) {
                if (QuadrangulationComputedTriangles.Contains(t))
                    continue;

                var u = Prng.InList(triangleGrid.NeighborsByTriangle[t]);
                if (QuadrangulationComputedTriangles.Contains(u))
                    continue;

                QuadrangulationComputedTriangles.Add(t);
                QuadrangulationComputedTriangles.Add(u);

                QuadrangulationVertices = new List<Vertex>(t.Vertices);
                QuadrangulationVertices.AddRange(u.Vertices);
                polygons.Add(Quadrangle.New(QuadrangulationVertices));
            }
            var orphanTriangles = triangleGrid.Triangles.FindAll(t => !QuadrangulationComputedTriangles.Contains(t));

            polygons.UnionWith(orphanTriangles);
            Profiler.EndSample();
            return polygons;
        }

        readonly HashSet<Quadrangle> SubQuads = new HashSet<Quadrangle>();
        HashSet<Quadrangle> SubQuadrangulate(params HashSet<Polygon>[] polygonLists) {
            Profiler.BeginSample("SubQuadrangulate", ProfilingTarget);

            SubQuads.Clear();
            foreach (var list in polygonLists)
                foreach (var q in list)
                    SubQuads.UnionWith(q.Quadrangulate());

            Profiler.EndSample();
            return SubQuads;
        }

        internal void SubQuadrangulate(int iterationCount = 1) {
            for (int i = 0; i < iterationCount; i++)
                Quadrangles = SubQuadrangulate(Quadrangles.Map(q => q as Polygon));
        }

        void ClearGeometry(bool totally = false) {
            Profiler.BeginSample("IrregularQuadGrid.ClearGeometry", ProfilingTarget);

            if (totally) Quadrangles.Clear();
            Edges.Clear();
            Vertices.Clear();

            QuadranglesByVertex.Clear();
            QuadranglesByEdge.Clear();
            EdgesByVertex.Clear();

            Profiler.EndSample();
        }

        public void ComputeGeometry() {
            ClearGeometry();

            Profiler.BeginSample("IrregularQuadGrid.ComputeGeometry", ProfilingTarget);

            foreach (var q in Quadrangles) {
                Vertices.UnionWith(q.Vertices);
                QuadranglesByVertex.AddToKeys(q.Vertices, q);

                Edges.UnionWith(q.Edges);
                QuadranglesByEdge.AddToKeys(q.Edges, q);

                foreach (var e in q.Edges) {
                    EdgesByVertex.AddToKey(e.A, e);
                    EdgesByVertex.AddToKey(e.B, e);
                }
            }
            Profiler.EndSample();

        }

        Quadrangle A;
        Quadrangle B;

        void ClearNeighboring() {
            Neighborings.Clear();//TODO pooling
            NeighborsByQuadrangle.Clear();
        }

        public void ComputeGeometryAndNeighboring() {
            Profiler.BeginSample("ComputeNeighboring", ProfilingTarget);

            ClearNeighboring();

            ComputeGeometry();

            foreach (var quadsByEdge in QuadranglesByEdge) {
                if (quadsByEdge.Value.Count < 2)
                    continue;

                A = quadsByEdge.Value[0];
                B = quadsByEdge.Value[1];

                NeighborsByQuadrangle.AddToKey(A, B);
                NeighborsByQuadrangle.AddToKey(B, A);
                Neighborings.Add(new Pair<Quadrangle>(A, B));
            }

            //Neighborings.Deduplicate();

            Profiler.EndSample();
        }

        void ClearBorders() {
            BorderQuads.Clear();
            BorderVertices.Clear();
            BorderEdges.Clear();
        }

        public void ComputeBorders() {
            Profiler.BeginSample("ComputeBorders", ProfilingTarget);

            ClearBorders();

            foreach (var kv in QuadranglesByEdge) {
                if (kv.Value.Count == 1) {
                    BorderEdges.Add(kv.Key);
                }
            }

            var deepBorderVerts = BorderEdges.Map(e => new List<Vertex>() { e.A, e.B });
            BorderVertices = new HashSet<Vertex>(deepBorderVerts.GetFlattened());

            BorderQuads = BorderEdges.Map(e => {
                var quads = QuadranglesByEdge[e];
                return quads[0];
            });

            Profiler.EndSample();
        }

        public Quadrangle GetEdgeNeighbor(Quadrangle quad, Edge edge) {
            return QuadranglesByEdge[edge].Find(q => q != quad);
        }

        public override string ToString() {
            return "IrregularQuadGrid";
        }

        public void RemoveQuad(Quadrangle quad) {
            Quadrangles.Remove(quad);
            BorderQuads.Remove(quad);
            quad.Vertices.ForEach(v => {
                if (QuadranglesByVertex[v].Count == 0) {
                    QuadranglesByVertex.Remove(v);
                    Vertices.Remove(v);
                    BorderVertices.Remove(v);
                }
            });
            quad.Edges.ForEach(e => {
                QuadranglesByEdge[e].Remove(quad);
                if (QuadranglesByEdge[e].Count == 0) {
                    QuadranglesByEdge.Remove(e);
                    Edges.Remove(e);
                    BorderEdges.Remove(e);
                }
            });
            Neighborings.RemoveWhere(n => n.Contains(quad));
            NeighborsByQuadrangle.Remove(quad);
            ComputeBorders();
        }

        public Quadrangle GetSideNeighbor(Quadrangle quad, Edge edge) {
            return QuadranglesByEdge[edge].Find(q => q != quad);
        }

        public HashSet<Quadrangle> ComputeSmoothestPathsFromQuad(Quadrangle startQuad, CardinalDirection direction, bool addStart = true) {
            var paths = new HashSet<Quadrangle> { startQuad };
            Vector2 vectorDirection = CardinalDirectionUtil.VectorByDirection[direction];
            var currentQuad = startQuad;
            var nextSide = currentQuad.GetSideTowards(vectorDirection);
            var nextQuad = GetSideNeighbor(currentQuad, nextSide);
            paths.Add(nextQuad);

            while (!(nextQuad is null)) {
                nextSide = nextQuad.GetOppositeSide(nextSide);
                nextQuad = GetSideNeighbor(nextQuad, nextSide);
                if (!(nextQuad is null)) paths.Add(nextQuad);
            }

            return paths;
        }
    }

}