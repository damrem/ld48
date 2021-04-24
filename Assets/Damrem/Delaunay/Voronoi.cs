using Damrem.DataStructures;
using Vertex = Damrem.Geom2D.Vertex;
using DamremTriangle = Damrem.Geom2D.Triangle;
using Damrem.Grids;
using Damrem.Collections;
using Damrem.UnityEngine;
using DelaunayEdge = csDelaunay.Edge;
using DelaunaySite = csDelaunay.Site;
using DelaunayTriangle = csDelaunay.Triangle;
using DelaunayVoronoi = csDelaunay.Voronoi;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Delaunay {
    public class Voronoi<TSiteData> : ITriangleGrid {
        readonly DelaunayVoronoi voronoi;

        public Dictionary<DelaunaySite, TSiteData> DataBySite { get; } = new Dictionary<DelaunaySite, TSiteData>();
        public IEnumerable<DelaunayEdge> Edges {
            get {
                return new HashSet<DelaunayEdge>(Sites.Map(site => site.Edges).GetFlattened());
            }
        }
        public Dictionary<DamremTriangle, List<DamremTriangle>> NeighborsByTriangle { get; } = new Dictionary<DamremTriangle, List<DamremTriangle>>();
        public Rect PlotBounds { get { return voronoi.PlotBounds.ToRect(); } }
        public List<DelaunaySite> Sites { get; private set; }
        public Dictionary<Vector2, DelaunaySite> SitesByPosition { get; private set; }

        public List<DamremTriangle> Triangles { get; }

        public Dictionary<DelaunaySite, List<DamremTriangle>> TrianglesBySite { get; private set; } = new Dictionary<DelaunaySite, List<DamremTriangle>>();

        public Voronoi(List<Vector2> points, Rect plotBounds, int lloydIterations = 0) {
            var pointsF = points.Map(v2 => v2.ToVector2f());
            voronoi = new DelaunayVoronoi(pointsF, plotBounds.ToRectf(), lloydIterations);
            SitesByPosition = new Dictionary<Vector2, DelaunaySite>(voronoi.SitesIndexedByLocation.ToVector2KeyedDictionary());
            Sites = SitesByPosition.GetValueList();
            Triangles = voronoi.Triangles.Map((DelaunayTriangle t) => {
                List<Vertex> corners = t.GetCoords().Map(coord => Vertex.Pool.TakeObject(coord));
                return new DamremTriangle(corners);
            });
        }

        public void ComputeTriangles() {
            foreach (DelaunaySite a in Sites) {
                var neighborsA = a.NeighborSites();
                foreach (DelaunaySite b in neighborsA) {
                    foreach (DelaunaySite c in b.NeighborSites()) {
                        if (!neighborsA.Contains(c))
                            continue;

                        var triangle = new DamremTriangle(new List<Vertex>() {
                            Vertex.Pool.TakeObject(a.Coord.ToVector2()),
                            Vertex.Pool.TakeObject(b.Coord.ToVector2()),
                            Vertex.Pool.TakeObject(c.Coord.ToVector2()),
                        });
                        if (Triangles.Contains(triangle))
                            continue;

                        Triangles.Add(triangle);

                        SaveTriangleForSite(a, triangle);
                        SaveTriangleForSite(b, triangle);
                        SaveTriangleForSite(c, triangle);
                    }
                }
                foreach (var t in TrianglesBySite[a]) {
                    foreach (var u in TrianglesBySite[a]) {
                        if (u == t)
                            continue;

                        if (u.IsAdjacentTo(t)) {
                            SaveNeighborForTriangle(t, u);
                            SaveNeighborForTriangle(u, t);
                        }
                    }
                }
            }
        }

        public HashSet<Pair<DamremTriangle>> TriangleNeighborPairs {
            get {
                var pairs = new HashSet<Pair<DamremTriangle>>();
                foreach (var set in NeighborsByTriangle) {
                    DamremTriangle t = set.Key;
                    foreach (DamremTriangle u in set.Value) {
                        pairs.Add(new Pair<DamremTriangle>(t, u));
                    }
                }
                return pairs;
            }
        }



        void SaveTriangleForSite(DelaunaySite s, DamremTriangle t) {
            if (!TrianglesBySite.ContainsKey(s))
                TrianglesBySite.Add(s, new List<DamremTriangle>());
            var triangles = TrianglesBySite[s];
            if (!triangles.Contains(t))
                triangles.Add(t);
        }

        void SaveNeighborForTriangle(DamremTriangle t, DamremTriangle neighbor) {
            if (!NeighborsByTriangle.ContainsKey(t))
                NeighborsByTriangle.Add(t, new List<DamremTriangle>());
            var neighbors = NeighborsByTriangle[t];
            if (!neighbors.Contains(neighbor))
                neighbors.Add(neighbor);
        }

        public bool AreNeighborSites(DelaunaySite a, DelaunaySite b) {
            return a != b
                && a != null
                && b != null
                && (
                    voronoi.NeighborSitesForSite(a.Coord).Contains(b.Coord)
                    || voronoi.NeighborSitesForSite(b.Coord).Contains(a.Coord)
                );
        }

        public DelaunayEdge CommonEdge(DelaunaySite a, DelaunaySite b) {
            if (!AreNeighborSites(a, b))
                return null;

            return a.Edges.Find(b.Edges.Contains);
        }

        public float DistanceBetweenSites(DelaunaySite a, DelaunaySite b) {
            return Vector2.Distance(a.Coord.ToVector2(), b.Coord.ToVector2());
        }

        public void SetSiteData(DelaunaySite site, TSiteData data) {
            DataBySite.Add(site, data);
        }

        public void RemoveSite(DelaunaySite site) {
            SitesByPosition.RemoveValue(site);
            DataBySite.Remove(site);
            Sites.Remove(site);
        }

        public void RemoveSitesAtBound() {
            Sites.FindAll(IsSiteAtBound).ForEach(RemoveSite);
        }

        public bool IsCoordAtBound(Vector2f coord) {
            if (coord.x <= PlotBounds.xMin || coord.x >= PlotBounds.xMax)
                return true;
            if (coord.y <= PlotBounds.yMin || coord.y >= PlotBounds.yMax)
                return true;
            return false;
        }

        public bool IsEdgeAtBound(DelaunayEdge edge) {
            if (IsCoordAtBound(edge.LeftEnd()) || IsCoordAtBound(edge.RightEnd()))
                return true;
            return false;
        }

        public bool IsSiteAtBound(DelaunaySite site) {
            return site.Edges.Exists(IsEdgeAtBound);
        }

        public DelaunaySite GetClosestSite(Vector2 from) {
            var sorted = new List<DelaunaySite>(Sites);
            sorted.Sort(delegate (DelaunaySite siteA, DelaunaySite siteB) {
                float distA = from.DistanceSquare(siteA.Coord.ToVector2());
                float distB = from.DistanceSquare(siteB.Coord.ToVector2());
                return Mathf.RoundToInt(distA - distB);
            });
            return sorted[0];
        }


    }

}