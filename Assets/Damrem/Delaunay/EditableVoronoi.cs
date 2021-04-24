using System.Collections.Generic;
using Damrem.Collections;
using csDelaunay;
using UnityEngine;

namespace Damrem.Delaunay {
    public class EditableVoronoi : Voronoi {
        public EditableVoronoi(List<Vector2f> points, Rectf plotBounds) : base(points, plotBounds) {

        }
        public EditableVoronoi(List<Vector2f> points, Rectf plotBounds, int lloydIterations) : base(points, plotBounds, lloydIterations) {

        }

        public void RemoveEdge(Edge edge) {
            //TODO removing edge requires deleting its reference from any associated voronoi object (site, edge, segment?)
            Debug.Log("RemoveEdge");
            if (edge.LeftSite != null) edge.LeftSite.Edges.Remove(edge);
            if (edge.RightSite != null) edge.RightSite.Edges.Remove(edge);
            Edges.Remove(edge);
        }

        public void RemoveSite(Site site) {
            //TODO removing site requires deleting its reference from any associated voronoi object (site, edge, segment?)
            Debug.Log("RemoveSite");
            List<Edge> edgesToRemove = new List<Edge>(site.Edges)
                .FindAll((Edge edge) => edge.IsPartOfConvexHull());
            Debug.Log(Edges.Count + " edges before in " + GetHashCode());

            edgesToRemove.ForEach(RemoveEdge);
            Debug.Log(Edges.Count + " edges after in " + GetHashCode());
            SitesIndexedByLocation.RemoveValue(site);
        }

        public void RemoveRectHull() {
            Debug.Log("RemoveRectHull");
            List<Site> allSites = this.GettSites();
            List<Site> hullSites = allSites.FindAll(site => site.IsPartOfHull(PlotBounds));
            Debug.Log(hullSites.Count + " hullSites");
            Debug.Log(allSites.Count + "allSites");
            foreach (Site site in hullSites)
                RemoveSite(site);
            Debug.Log(this.GettSites().Count + " sites end");
            RemoveOrphanEdges();
        }

        void RemoveOrphanEdges() {
            Debug.Log(Edges.FindAll(edge => edge.LeftSite == null && edge.RightSite == null).Count);
            Edges.FindAll(edge => edge.LeftSite == null && edge.RightSite == null).ForEach(RemoveEdge);
        }

    }
}