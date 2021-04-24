using Damrem.Collections;
using Damrem.Geom2D;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Damrem.Grids.Irregular {

    [CreateAssetMenu(fileName = "Grid", menuName = "Grid/Grid Scriptable Object")]
    public class GridSO : ScriptableObject {

        public double GeneratedInSecs;

        public List<Vector2> Vertices;
        public List<int> Quads;
        public List<int> Edges;

        /// <summary>
        /// A list formatted like this:
        /// 0: index of a vertex
        /// 1: index of a quad containing the vertex
        /// 2: index of another quad containing the vertex
        /// 3: ...
        /// 4: -1 as a separator
        /// 5: index of another vertex
        /// 6: index of a quad containing the other vertex
        /// 7: ...
        /// </summary>
        // public List<int> EdgesByVertex;
        // public List<int> QuadsByVertex;
        // public List<int> QuadsByEdge;

        public List<int> HullVertices;
        public List<int> HullEdges;
        public List<int> HullQuads;
        public float AverageEdgeSize;

        public void Clear() {
            Vertices.Clear();
            Edges.Clear();
            Quads.Clear();

            //   EdgesByVertex.Clear();
            //   QuadsByVertex.Clear();
            //   QuadsByEdge.Clear();

            HullVertices.Clear();
            HullEdges.Clear();
            HullQuads.Clear();
        }

        public void SetGrid(IrregularQuadGrid grid) {
            Vertices = grid.Vertices.ToList().Map(v => v.Position);

            Quads = new List<int>();
            foreach (var q in grid.Quadrangles) {
                var vertices = q.Vertices.Map(GetVertexIndex);
                Quads.AddRange(vertices);
            }

            Edges = new List<int>();
            float averageEdgeSize = 0;
            foreach (var e in grid.Edges) {
                Edges.Add(GetVertexIndex(e.A));
                Edges.Add(GetVertexIndex(e.B));
                averageEdgeSize += e.Size;
            }
            averageEdgeSize /= Edges.Count;
            AverageEdgeSize = averageEdgeSize;
        }

        int GetVertexIndex(Vertex corner) {
            return Vertices.IndexOf(corner.Position);
        }

        public IrregularQuadGrid ToGrid() {
            var vertices = new HashSet<Vertex>(Vertices.Map(Vertex.New));

            var quads = new HashSet<Quadrangle>();
            for (int i = 0; i < Quads.Count; i += 4) {
                quads.Add(Quadrangle.New(new List<Vertex> {
                    Vertex.New(Vertices[Quads[i]]),
                    Vertex.New(Vertices[Quads[i+1]]),
                    Vertex.New(Vertices[Quads[i+2]]),
                    Vertex.New(Vertices[Quads[i+3]]),
                }));
            }

            var edges = new HashSet<Edge>();
            for (int i = 0; i < Edges.Count; i += 2) {
                var a = Vertex.New(Vertices[Edges[i]]);
                var b = Vertex.New(Vertices[Edges[i + 1]]);
                edges.Add(new Edge(a, b));
            }

            return new IrregularQuadGrid(vertices, quads, edges);
        }
    }
}